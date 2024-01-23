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
    
    public bool Equals(string key)
    {
        string data = (string)Data!;
        return data[1..^1] == key;
    }

    public override string ToString()
    {
        return Data is null ? string.Empty : (string)Data;
    }
}