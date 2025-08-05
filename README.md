# DCIT 318 – Programming II: Assignment 3

This repository contains solutions for **Assignment 3**, demonstrating advanced C# concepts across five mini-projects:

## 📂 Contents

1. **FinanceManagementSystem** (Question 1)  
   - Uses `record`, `interface`, and `sealed` class.  
   - Simulates three transaction types (Bank, MobileMoney, Crypto) against a `SavingsAccount`.

2. **HealthManagementSystem** (Question 2)  
   - Implements a generic `Repository<T>` for `Patient` and `Prescription`.  
   - Groups prescriptions by patient and displays them.

3. **WarehouseInventorySystem** (Question 3)  
   - Defines `IInventoryItem`, two product classes, and a generic `InventoryRepository<T>`.  
   - Custom exceptions for duplicates, missing items, and invalid quantities.  
   - `WareHouseManager` seeds data and demonstrates exception handling.

4. **SchoolGradingManagementSystem** (Question 4)  
   - Reads `input.txt`, validates each line (`Student` + custom exceptions), assigns grades, and writes `report.txt`.  
   - Handles missing files, malformed scores, and incomplete records.

5. **InventoryLoggerApp** (Question 5)  
   - Uses an immutable `record InventoryItem` and a marker interface.  
   - `InventoryLogger<T>` persists data to JSON, loads it back, and verifies recovery.

---

> [!NOTE]
> All code was developed under my personal GitHub account but authored with my student email, for submission compliance. You can verify each commit’s author in the history.

---

## ▶ How to Run Each Project

Each folder is a standalone `.NET` console app:

1. Open a folder (e.g. `FinanceManagementSystem`) in Visual Studio or via CLI.  
2. If using CLI:
   ```bash
   cd FinanceManagementSystem
   dotnet run
