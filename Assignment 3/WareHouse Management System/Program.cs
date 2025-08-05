using System;
using System.Collections.Generic;

// 1. Interface
interface IInventoryItem
{
    int Id { get; }
    string Name { get; }
    int Quantity { get; set; }
}

// 2. Electronic Item
class ElectronicItem : IInventoryItem
{
    public int Id { get; }
    public string Name { get; }
    public int Quantity { get; set; }
    public string Brand { get; }
    public int WarrantyMonths { get; }

    public ElectronicItem(int id, string name, int quantity, string brand, int warrantyMonths)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Brand = brand;
        WarrantyMonths = warrantyMonths;
    }

    public override string ToString() =>
        $"{Name} (ID: {Id}, Brand: {Brand}, Qty: {Quantity}, Warranty: {WarrantyMonths} months)";
}

// 3. Grocery Item
class GroceryItem : IInventoryItem
{
    public int Id { get; }
    public string Name { get; }
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; }

    public GroceryItem(int id, string name, int quantity, DateTime expiryDate)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        ExpiryDate = expiryDate;
    }

    public override string ToString() =>
        $"{Name} (ID: {Id}, Qty: {Quantity}, Exp: {ExpiryDate:yyyy-MM-dd})";
}

// 4. Custom Exceptions
class DuplicateItemException : Exception
{
    public DuplicateItemException(string message) : base(message) { }
}

class ItemNotFoundException : Exception
{
    public ItemNotFoundException(string message) : base(message) { }
}

class InvalidQuantityException : Exception
{
    public InvalidQuantityException(string message) : base(message) { }
}

// 5. Generic Inventory Repository
class InventoryRepository<T> where T : IInventoryItem
{
    private Dictionary<int, T> _items = new();

    public void AddItem(T item)
    {
        if (_items.ContainsKey(item.Id))
            throw new DuplicateItemException($"Item with ID {item.Id} already exists.");
        _items[item.Id] = item;
    }

    public T GetItemById(int id)
    {
        if (!_items.TryGetValue(id, out var item))
            throw new ItemNotFoundException($"Item with ID {id} not found.");
        return item;
    }

    public void RemoveItem(int id)
    {
        if (!_items.ContainsKey(id))
            throw new ItemNotFoundException($"Cannot remove. Item with ID {id} not found.");
        _items.Remove(id);
    }

    public List<T> GetAllItems() => new(_items.Values);

    public void UpdateQuantity(int id, int newQuantity)
    {
        if (newQuantity < 0)
            throw new InvalidQuantityException("Quantity cannot be negative.");
        if (!_items.ContainsKey(id))
            throw new ItemNotFoundException($"Item with ID {id} not found.");
        _items[id].Quantity = newQuantity;
    }
}

// 6. WarehouseManager
class WareHouseManager
{
    private InventoryRepository<ElectronicItem> _electronics = new();
    private InventoryRepository<GroceryItem> _groceries = new();

    public void SeedData()
    {
        try
        {
            _electronics.AddItem(new ElectronicItem(1, "Laptop", 5, "Dell", 24));
            _electronics.AddItem(new ElectronicItem(2, "Smartphone", 10, "Samsung", 12));
            _groceries.AddItem(new GroceryItem(1, "Milk", 20, DateTime.Today.AddDays(7)));
            _groceries.AddItem(new GroceryItem(2, "Bread", 15, DateTime.Today.AddDays(2)));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Seed Error] {ex.Message}");
        }
    }

    public void PrintAllItems<T>(InventoryRepository<T> repo) where T : IInventoryItem
    {
        foreach (var item in repo.GetAllItems())
        {
            Console.WriteLine($"- {item}");
        }
    }

    public void IncreaseStock<T>(InventoryRepository<T> repo, int id, int quantity) where T : IInventoryItem
    {
        try
        {
            var item = repo.GetItemById(id);
            repo.UpdateQuantity(id, item.Quantity + quantity);
            Console.WriteLine($"[+] Stock increased. New quantity: {item.Quantity}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] {ex.Message}");
        }
    }

    public void RemoveItemById<T>(InventoryRepository<T> repo, int id) where T : IInventoryItem
    {
        try
        {
            repo.RemoveItem(id);
            Console.WriteLine($"[-] Item {id} removed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] {ex.Message}");
        }
    }

    public InventoryRepository<ElectronicItem> Electronics => _electronics;
    public InventoryRepository<GroceryItem> Groceries => _groceries;
}

// 7. Main Program
class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine(@"
  ╔══════════════════════════════════════╗
  ║     WAREHOUSE INVENTORY MANAGER     ║
  ╚══════════════════════════════════════╝");

        var manager = new WareHouseManager();
        manager.SeedData();

        Console.WriteLine("\n📦 Grocery Items:");
        manager.PrintAllItems(manager.Groceries);

        Console.WriteLine("\n🔌 Electronic Items:");
        manager.PrintAllItems(manager.Electronics);

        Console.WriteLine("\n❌ Try adding duplicate item:");
        try
        {
            manager.Electronics.AddItem(new ElectronicItem(1, "Tablet", 3, "Lenovo", 18));
        }
        catch (DuplicateItemException ex)
        {
            Console.WriteLine($"[Handled] {ex.Message}");
        }

        Console.WriteLine("\n🗑️ Try removing non-existent item:");
        manager.RemoveItemById(manager.Groceries, 999);

        Console.WriteLine("\n🔁 Try updating with invalid quantity:");
        try
        {
            manager.Electronics.UpdateQuantity(2, -5);
        }
        catch (InvalidQuantityException ex)
        {
            Console.WriteLine($"[Handled] {ex.Message}");
        }

        Console.WriteLine("\n✅ Program complete.");
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
