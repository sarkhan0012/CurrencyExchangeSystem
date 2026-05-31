using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CurrencyExchange.WcfService
{
    public class Service1 : IService1
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public bool RegisterUser(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Users (Username, Password) VALUES (@username, @password)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        public int LoginUser(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Id FROM Users WHERE Username=@username AND Password=@password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                    return 0;
                }
            }
        }

        public decimal GetBalance(int userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Balance FROM Users WHERE Id=@userId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToDecimal(result);
                    }
                    return 0.00m;
                }
            }
        }

        public bool TopUpWallet(int userId, decimal amount)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string updateQuery = "UPDATE Users SET Balance = Balance + @amount WHERE Id=@userId";
                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        string insertQuery = "INSERT INTO Transactions (UserId, TransactionType, CurrencyCode, Amount, Rate) VALUES (@userId, 'TOPUP', 'PLN', @amount, 1)";
                        using (SqlCommand cmd2 = new SqlCommand(insertQuery, conn))
                        {
                            cmd2.Parameters.AddWithValue("@userId", userId);
                            cmd2.Parameters.AddWithValue("@amount", amount);
                            cmd2.ExecuteNonQuery();
                        }
                        return true;
                    }
                    return false;
                }
            }
        }

        public bool ProcessTransaction(int userId, string transactionType, string currencyCode, decimal amount, decimal rate)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                decimal totalValue = amount * rate;
                string updateQuery = "";

                if (transactionType == "BUY")
                {
                    updateQuery = "UPDATE Users SET Balance = Balance - @totalValue WHERE Id=@userId AND Balance >= @totalValue";
                }
                else if (transactionType == "SELL")
                {
                    updateQuery = "UPDATE Users SET Balance = Balance + @totalValue WHERE Id=@userId";
                }

                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@totalValue", totalValue);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        string insertQuery = "INSERT INTO Transactions (UserId, TransactionType, CurrencyCode, Amount, Rate) VALUES (@userId, @type, @currency, @amount, @rate)";
                        using (SqlCommand cmd2 = new SqlCommand(insertQuery, conn))
                        {
                            cmd2.Parameters.AddWithValue("@userId", userId);
                            cmd2.Parameters.AddWithValue("@type", transactionType);
                            cmd2.Parameters.AddWithValue("@currency", currencyCode);
                            cmd2.Parameters.AddWithValue("@amount", amount);
                            cmd2.Parameters.AddWithValue("@rate", rate);
                            cmd2.ExecuteNonQuery();
                        }
                        return true;
                    }
                    return false;
                }
            }
        }

        public List<TransactionRecord> GetTransactionHistory(int userId)
        {
            List<TransactionRecord> history = new List<TransactionRecord>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT TransactionType, CurrencyCode, Amount, Rate, TransactionDate FROM Transactions WHERE UserId=@userId ORDER BY TransactionDate DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            history.Add(new TransactionRecord
                            {
                                TransactionType = reader["TransactionType"].ToString(),
                                CurrencyCode = reader["CurrencyCode"].ToString(),
                                Amount = Convert.ToDecimal(reader["Amount"]),
                                Rate = Convert.ToDecimal(reader["Rate"]),
                                TransactionDate = Convert.ToDateTime(reader["TransactionDate"])
                            });
                        }
                    }
                }
            }
            return history;
        }
    }
}

