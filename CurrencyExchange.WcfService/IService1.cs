using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace CurrencyExchange.WcfService
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        bool RegisterUser(string username, string password);

        [OperationContract]
        int LoginUser(string username, string password);

        [OperationContract]
        decimal GetBalance(int userId);

        [OperationContract]
        bool TopUpWallet(int userId, decimal amount);

        [OperationContract]
        bool ProcessTransaction(int userId, string transactionType, string currencyCode, decimal amount, decimal rate);

        [OperationContract]
        List<TransactionRecord> GetTransactionHistory(int userId);
    }

    [DataContract]
    public class TransactionRecord
    {
        [DataMember]
        public string TransactionType { get; set; }

        [DataMember]
        public string CurrencyCode { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal Rate { get; set; }

        [DataMember]
        public DateTime TransactionDate { get; set; }
    }
}