namespace JsonLib.Nodes;

/// <summary>
/// Node for numbers.
/// </summary>
public class NumberNode : BaseNode
{
    public NumberNode(decimal data, BaseNode? parent = null) 
        : base(data, parent)
    {
        
    }
}