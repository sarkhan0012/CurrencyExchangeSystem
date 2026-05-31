# .NET Programming Final Project
## 🏦 ExchangeApp - Currency Exchange Office System

---

* **Name:** Sarkhan Rahimov
* **Student ID:** 64293

---

### 📘 Course Information
* **Course Name:** .NET Programming
* **Instructor:** Marcin Krupski
* **Academic Evaluation:** Satisfies Object-Oriented Programming (OOP) lab structures, WCF-WPF communication protocols, and clean code standards.

### 📝 Project Title
**Distributed Currency Exchange System using WCF, WPF, and SQL Server**

---

### 🚀 Project Description
This project is a comprehensive distributed desktop application developed as the final showcase for the **.NET Programming** course. It implements a service-oriented architecture (SOA) where business logic, bank API integration, and user data management are handled securely by a backend **WCF Service**, while a desktop **WPF Client** provides a modern, reactive user interface for operations.

**Key Features & Implemented Concepts:**
* **Live NBP Data Integration:** Fetches real-time exchange rates directly from the public **National Bank of Poland (NBP) API** via secure HTTPS connections and parses them using `Newtonsoft.Json`.
* **User Management & Virtual Wallet:** Features a secure user account infrastructure supporting registration, login states, and live PLN balance top-ups.
* **Real-time Currency Trading:** Users can dynamically buy and sell foreign currencies (USD, EUR, GBP, etc.) based on real-time central bank rates.
* **Transaction Ledger (History):** Every buy, sell, and top-up operation is written directly to an isolated SQL database ledger, allowing users to view their complete transaction history securely on the dashboard.
* **Clean Code & Error Handling:** Implements robust `try-catch` exception handling for network availability, database connection drops, and invalid inputs, ensuring the core application workflow remains stable.

---

### 🛠 Core OOP & Architecture Implementations
Following the absolute criteria set by the Course Directorate and the Instructor, the system heavily incorporates core Object-Oriented Programming principles:
* **Encapsulation & Data Models:** Full data shielding using explicit access modifiers and Data Contracts (`[DataContract]`, `[DataMember]`).
* **Interfaces & Contracts (Polymorphism):** Complete separation of concerns achieved via service contract definitions utilizing the **`IService1` Interface** (`[ServiceContract]`, `[OperationContract]`).
* **Generic Collections:** Dynamic UI data binding using type-safe `List<T>` for transaction histories and NBP API data mapping to avoid loose object arrays and casting errors.

---

### 📂 Repository Structure
* **`CurrencyExchange.WcfService/`**: Backend WCF Web Service layer hosting operational endpoints, tracking local MSSQL parameters, and handling DB interactions.
* **`CurrencyExchange.WPF/`**: Desktop user interface built with WPF/XAML, managing reactive event handlers and secure proxy channels (`ServiceReference`).

---

### 💻 How to Run the Project

1. **Database Setup:**
   * The application uses natively **SQL Server LocalDB** (`(localdb)\MSSQLLocalDB`).
   * Execute the SQL script provided in the project to create the `CurrencyExchangeDB` database, along with the `Users` and `Transactions` tables.

2. **Visual Studio Multi-Startup Configuration:**
   * Open the primary solution file (`CurrencyExchangeSystem.sln` / `ExchangeApp.sln`) inside Visual Studio.
   * Right-click the top-level **Solution** node in your Solution Explorer and open **Properties** -> **Startup Project**.
   * Choose **Multiple startup projects**.
   * Toggle the action settings for both **`CurrencyExchange.WcfService`** and **`CurrencyExchange.WPF`** to **Start**.
   * Select **Apply** and press **OK**.

3. **Execution:**
   * Click the green **Start** button or press **F5**. Visual Studio will boot up the background WCF service host environment and initialize your WPF application workspace simultaneously.
   * Register a new account, sign in, add some virtual PLN tokens, and enter currency codes (USD, EUR) to monitor live exchange balances and execute operations!

---

### 🛠 Technologies Used
* **Development Environment:** Visual Studio 2022, .NET Framework 4.7.2
* **Backend Framework:** WCF (Windows Communication Foundation)
* **Frontend Framework:** WPF (Windows Presentation Foundation), XAML
* **Database Client:** Microsoft SQL Server (LocalDB), ADO.NET Core Data APIs (`SqlConnection`, `SqlCommand`)
* **External API Handshake:** National Bank of Poland JSON Feed (Deserialized via `Newtonsoft.Json`)
