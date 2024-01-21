namespace JsonLib;

/// <summary>
/// JSON object model.
/// </summary>
public class Product
{
    public int ProductId { get; }
    public string ProductName { get; }
    public string Category { get; }
    public decimal Price { get; }
    public int QuantityInStock { get; }
    public bool IsDiscounted { get; }
    public List<string> Reviews { get; }

    public Product (
        int productId, 
        string productName, 
        string category, 
        decimal price, 
        int quantityInStock, 
        bool isDiscounted, 
        List<string> reviews)
    {
        ProductId = productId;
        ProductName = productName;
        Category = category;
        Price = price;
        QuantityInStock = quantityInStock;
        IsDiscounted = isDiscounted;
        Reviews = reviews;
    }
}