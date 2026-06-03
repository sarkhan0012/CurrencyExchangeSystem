<img width="1912" height="1029" alt="Screenshot 2026-06-03 154719" src="https://github.com/user-attachments/assets/88865c54-325c-48f6-9b44-0d7d72401dc1" />
# Network Application Development Project

* **Name:** Sarkhan Rahimov
* **Student ID:** 63294

## 🏦 Currency Exchange Office System

### 📘 Course Information
* **Course Name:** Network Application Development

### 📝 Project Title
**Distributed Currency Exchange System using WCF, WPF, and SQL Server**

---

![.NET Framework](https://img.shields.io/badge/.NET-4.7.2-512BD4?logo=dotnet)
![WPF](https://img.shields.io/badge/Frontend-WPF-blue)
![WCF](https://img.shields.io/badge/Backend-WCF-green)
![SQL](https://img.shields.io/badge/Database-LocalDB-lightgrey)

### 📖 Project Overview
**ExchangeApp** is a comprehensive distributed network application built on a robust Client-Server architecture. Developed as the final project for the *.Network Application Development* course, this system simulates a real-world Currency Exchange Office. 

The project separates the business logic, external API integrations, and database management into a secure backend **WCF (Windows Communication Foundation) Service**, while presenting a highly reactive and user-friendly interface via a **WPF (Windows Presentation Foundation)** desktop client.

### ✨ Key Features
* **🌐 Live Network API Integration:** Securely fetches real-time exchange rates (Bid/Ask) from the **National Bank of Poland (NBP) API** over HTTPS, parsing JSON payloads dynamically.
* **🔗 WCF Service-Oriented Architecture:** Fully decoupled architecture where the WPF client communicates with the WCF backend via strictly defined Service Contracts (`IService1`) and Endpoints.
* **💼 Virtual Wallet & User Sessions:** Includes a robust user authentication system. Users can register, log in, and top up their virtual PLN balances in real-time over the network.
* **💱 Live Currency Trading:** Users can execute Buy and Sell operations for foreign currencies (USD, EUR, GBP, CHF, etc.) based on the live mid-rates retrieved from the NBP network.
* **🛡️ Secure Transaction Ledger:** Every financial operation is isolated and permanently recorded in a Microsoft SQL Server database, providing users with an accurate, real-time transaction history ledger.
* **🛑 Advanced Error Handling:** Implements clean code principles with robust exception handling for network drops, endpoint misconfigurations, and invalid user inputs.

---

### 🛠️ System Architecture & Technologies

The application is strictly divided into distinct network layers:

1. **Backend / Network Service (`CurrencyExchange.WcfService`):**
   * Acts as the central nervous system.
   * Handles direct connections to the NBP API and the local SQL Database.
   * Technologies: **C#, WCF, ADO.NET, Newtonsoft.Json**

2. **Frontend / Client UI (`CurrencyExchange.WPF`):**
   * Acts as the consumer of the WCF network service.
   * Maintains state and manages the user interface reactively.
   * Technologies: **C#, WPF, XAML**

3. **Database Layer:**
   * **Microsoft SQL Server (LocalDB)** handles persistent data storage.

---

### 🚀 How to Run the Project Locally

To run this distributed application on your local machine, both the Server (WCF) and the Client (WPF) must be started simultaneously.

**1. Database Setup**
* The application connects natively to **SQL Server LocalDB** (`(localdb)\MSSQLLocalDB`).
* Ensure that the `CurrencyExchangeDB` database is created on your local SQL server with the required `Users` and `Transactions` tables before running the service.

**2. Multi-Startup Configuration in Visual Studio**
* Open the `CurrencyExchangeSystem.slnx` solution file in Visual Studio.
* Right-click the **Solution** node in the *Solution Explorer* and select **Properties**.
* Navigate to **Startup Project** and select **Multiple startup projects**.
* Set the Action to **Start** for both `CurrencyExchange.WcfService` and `CurrencyExchange.WPF`.
* Click **Apply** and **OK**.

**3. Execution**
* Press **F5** or click the green **Start** button.
* Visual Studio will launch the WCF service environment in the background and open the WPF graphical interface.
* Create a new account, log in, top up your balance, and start trading!
