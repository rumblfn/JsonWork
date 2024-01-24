using System.Text;

namespace JsonLib.Nodes;

/// <summary>
/// Provides array of nodes.
/// </summary>
public class ListNode : BaseNode
{
    public ListNode(BaseNode? parent = null) 
        : base(parent)
    {
        Data = new List<BaseNode>();
    }

    public List<BaseNode>? GetData()
    {
        return Data as List<BaseNode>;
    }

    /// <summary>
    /// Adds new node to collection.
    /// </summary>
    /// <param name="node">Node to add.</param>
    /// <returns>Added node.</returns>
    /// <exception cref="Exception">If collection if null.</exception>
    public BaseNode Add(BaseNode node)
    {
        if (Data is not List<BaseNode> data)
        {
            throw new Exception();
        }
        
        data.Add(node);
        return node is DictionaryNode or ListNode ? node : this;
    }

    /// <summary>
    /// Provide a string representation of the List data.
    /// </summary>
    /// <returns>List in string view.</returns>
    /// <exception cref="Exception">Error type of Data.</exception>
    public override string ToString()
    {
        var sb = new StringBuilder();
        
        if (Data is not List<BaseNode> data)
        {
            throw new Exception();
        }

        for (int i = 0; i < data.Count; i++)
        {
            sb.Append($"{i + 1}. " + data[i] + Environment.NewLine);
        }
        
        return $"Collection: [{Environment.NewLine}" + sb + "]";
    }
}