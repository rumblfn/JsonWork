using System.Text;

namespace JsonLib.Nodes;

/// <summary>
/// Node with parent and child members for handling dictionary data.
/// Provides pending key to adding values with saved key.
/// </summary>
public class DictionaryNode : BaseNode
{
    private StringNode? PendingKey { get; set; }
    
    public DictionaryNode(BaseNode? parent = null) 
        : base(parent)
    {
        Data = new Dictionary<StringNode, BaseNode>();
    }

    
    /// <summary>
    /// Method for adding nodes.
    /// </summary>
    /// <param name="node">Node to add.</param>
    /// <returns>Node specified by calling methods.</returns>
    public BaseNode Add(BaseNode node)
    {
        return node is StringNode ? AddKeyOrValue(node) : AddWithKeyFromCache(node);
    }

    /// <summary>
    /// Handles new value, sets it to pendingKey if it null.
    /// Or adding new value with saved key.
    /// </summary>
    /// <param name="node">Node to add.</param>
    /// <returns>Added node.</returns>
    /// <exception cref="Exception">Error with adding node.</exception>
    private BaseNode AddKeyOrValue(BaseNode node)
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
        return this;
    }

    /// <summary>
    /// Add data by PendingKey <see cref="PendingKey"/>.
    /// </summary>
    /// <param name="node">Any child node.</param>
    /// <returns>Added child.</returns>
    /// <exception cref="Exception">If cache is empty</exception>
    private BaseNode AddWithKeyFromCache(BaseNode node)
    {
        if (PendingKey == null)
        {
            throw new Exception();
        }
        
        this[PendingKey] = node;
        
        // Clean cache.
        PendingKey = null;
        return node is DictionaryNode or ListNode ? node : this;
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
    
    /// <summary>
    /// Provide a string representation of the dictionary data.
    /// </summary>
    /// <returns>Dictionary in string view.</returns>
    /// <exception cref="Exception">Error type of Data.</exception>
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