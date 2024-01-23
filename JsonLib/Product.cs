namespace JsonLib;

/// <summary>
/// JSON object model.
/// </summary>
public class Product
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Category { get; init; }
    public double Price { get; init; }
    public int QuantityInStock { get; init; }
    public bool IsDiscounted { get; init; }
    public List<string>? Reviews { get; init; }
}