using System.Globalization;

namespace JsonLib.Nodes;

/// <summary>
/// Node for numbers.
/// </summary>
public class NumberNode : BaseNode
{
    public NumberNode(double data, BaseNode? parent = null) 
        : base(data, parent)
    {
        
    }
    
    public override string ToString()
    {
        return Data is null
            ? string.Empty
            : ((decimal)Data).ToString(CultureInfo.InvariantCulture);
    }
}