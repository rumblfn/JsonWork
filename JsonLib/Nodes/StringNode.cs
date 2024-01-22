namespace JsonLib.Nodes;

/// <summary>
/// Node for strings.
/// </summary>
public class StringNode : BaseNode
{
    public StringNode(string data, BaseNode? parent = null) 
        : base(data, parent)
    {
        
    }

    public override string ToString()
    {
        if (Data is null)
        {
            return string.Empty;
        }
        
        return (string)Data;
    }
}