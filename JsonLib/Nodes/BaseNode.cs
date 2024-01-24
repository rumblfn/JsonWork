namespace JsonLib.Nodes;

/// <summary>
/// Base node class. It provides properties for storing data
/// and maintaining a parent-child relationship between nodes.
/// </summary>
public abstract class BaseNode
{
    public object? Data { get; protected init; }
    public BaseNode? Parent { get; }

    protected BaseNode(BaseNode? parent = null)
    {
        Data = null;
        Parent = parent;
    }
}