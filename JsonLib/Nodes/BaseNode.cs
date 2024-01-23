namespace JsonLib.Nodes;

/// <summary>
/// Base node class.
/// Provides parent and child nodes.
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