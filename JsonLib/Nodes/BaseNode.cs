namespace JsonLib.Nodes;

/// <summary>
/// Base node class. It provides properties for storing data
/// and maintaining a parent-child relationship between nodes.
/// </summary>
public abstract class BaseNode
{
    public object? Data { get; }
    public BaseNode? Parent { get; }

    protected BaseNode(object? data, BaseNode? parent = null)
    {
        Data = data;
        Parent = parent;
    }
}