using System.Text;

namespace JsonLib.Nodes;

/// <summary>
/// Node with parent and child members for handling dictionary data.
/// Provides pending key to adding values with saved key.
/// </summary>
public class DictionaryNode : BaseNode
{
    private StringNode? PendingKey { get; set; }
    
    public DictionaryNode(Dictionary<StringNode, BaseNode> data, BaseNode? parent = null) 
        : base(data, parent)
    {
        
    }

    /// <summary>
    /// Handles new value, sets it to pendingKey if it null.
    /// Or adding new value with saved key.
    /// </summary>
    /// <param name="node">Node to add.</param>
    /// <returns>Added node.</returns>
    /// <exception cref="Exception">Error with adding node.</exception>
    public BaseNode AddKeyOrValue(BaseNode node)
    {
        if (PendingKey != null)
        {
            return AddWithKeyFromCache(node);
        }
        
        if (node is not StringNode stringNode)
        {
            throw new Exception();
        }
            
        PendingKey = stringNode;
        return stringNode;
    }

    /// <summary>
    /// Add data by PendingKey <see cref="PendingKey"/>.
    /// </summary>
    /// <param name="node">Any child node.</param>
    /// <returns>Added child.</returns>
    /// <exception cref="Exception">If cache is empty</exception>
    public BaseNode AddWithKeyFromCache(BaseNode node)
    {
        if (PendingKey == null)
        {
            throw new Exception();
        }
        
        this[PendingKey] = node;
        
        // Clean cache.
        PendingKey = null;
        
        return node;
    }
    
    /// <summary>
    /// Setting data by key.
    /// </summary>
    /// <param name="key">Dictionary key.</param>
    public BaseNode? this[StringNode key]
    {
        set
        {
            if (Data is Dictionary<StringNode, BaseNode?> data)
            {
                data[key] = value;
            }
        }
    }
    
    /// <summary>
    /// Accessing to data by indexing.
    /// </summary>
    /// <param name="key">Dictionary key.</param>
    public BaseNode? this[string key]
    {
        get
        {
            if (Data is Dictionary<StringNode, BaseNode?> data)
            {
                return (
                    from kvp in data 
                    where kvp.Key.Equals(key) 
                    select kvp.Value
                    ).First();
            }
            return null;
        }
    }
    
    public override string ToString()
    {
        var sb = new StringBuilder();
        if (Data is not Dictionary<StringNode, BaseNode> data)
        {
            throw new Exception();
        }

        foreach (KeyValuePair<StringNode, BaseNode> kvp in data)
        {
            sb.Append($"{kvp.Key}: " + kvp.Value + Environment.NewLine);
        }
        
        return $"Collection: {{{Environment.NewLine}" + sb + "}";
    }
}