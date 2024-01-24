using System.Globalization;

namespace JsonLib.Nodes;

/// <summary>
/// Boolean node for handling boolean and null values.
/// </summary>
public class BooleanNode: BaseNode
{
    public BooleanNode(bool? data, BaseNode? parent = null)
        : base(data, parent)
    {
        
    }
    
    /// <summary>
    /// Convert the boolean value to a string representation.
    /// </summary>
    /// <returns>Boolean string.</returns>
    public override string ToString()
    {
        return Data is null
            ? string.Empty
            : ((bool)Data).ToString(CultureInfo.InvariantCulture);
    }
}