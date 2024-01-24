using System.Globalization;
using JsonLib.Nodes;
using System.Text;
using Utils;

namespace JsonLib;

/// <summary>
/// Class for reading/writing json.
/// </summary>
public static class JsonParser
{
    private const char ArrayEnd = ']';
    private const char ArrayStart = '[';
    
    private const char ObjectEnd = '}';
    private const char ObjectStart = '{';
    
    private const char WhiteSpace = ' ';
    private const char KeyValueSeparator = ':';
    private const char ElementsSeparator = ',';
    private const char Tab = '\t';

    private const char Quote = '"';
    private const char QuoteEscape = '\\';

    private const string NullValue = "null";
    private const string BooleanTrue = "true";
    private const string BooleanFalse = "false";

    private static readonly char[] QuoteEndings = { Quote };
    private static readonly char[] Escapes = { QuoteEscape };
    private static readonly char[] TypeEndings = { ElementsSeparator, ArrayEnd, ObjectEnd, WhiteSpace };
    
    /// <summary>
    /// Use for writing dictionary key value pairs with formatting.
    /// </summary>
    /// <param name="key">Key in pair.</param>
    /// <param name="value">Value in pair.</param>
    private static void WriteKeyValuePair(string key, string? value)
    {
        Console.WriteLine($@"{Tab}{Tab}{Quote}{key}{Quote}{KeyValueSeparator} {value}{ElementsSeparator}");
    }

    /// <summary>
    /// JSON format specify ending of elements.
    /// Last element must not have comma <see cref="ElementsSeparator"/>.
    /// Method writes elements specified by their indexes. It checks if element is last.
    /// </summary>
    /// <param name="currentIndex">Current element index.</param>
    /// <param name="lastIndex">Last element index.</param>
    private static void HandleLastElementEnding(int currentIndex, int lastIndex)
    {
        if (currentIndex != lastIndex)
        {
            Console.Write(ElementsSeparator);
        }
            
        Console.Write(Environment.NewLine);
    }
    
    /// <summary>
    /// Method for writing specified by work data in json format.
    /// </summary>
    /// <param name="products">List of products.</param>
    public static void WriteJson(List<Product> products)
    {
        Console.WriteLine(ArrayStart);

        int lastProductIndex = products.Count - 1;
        for (int i = 0 ; i <= lastProductIndex; i++)
        {
            Product product = products[i];
            Console.WriteLine($@"{Tab}{ObjectStart}");
            
            WriteKeyValuePair("product_id", product.Id.ToString());
            WriteKeyValuePair("product_name", product.Name);
            WriteKeyValuePair("category", product.Category);
            WriteKeyValuePair("price", product.Price.ToString(CultureInfo.InvariantCulture));
            WriteKeyValuePair("quantity_in_stock", product.QuantityInStock.ToString());

            Console.WriteLine(product.IsDiscounted
                ? $@"{Tab}{Tab}{Quote}is_discounted{Quote}{KeyValueSeparator} {BooleanTrue}{ElementsSeparator}"
                : $@"{Tab}{Tab}{Quote}is_discounted{Quote}{KeyValueSeparator} {BooleanFalse}{ElementsSeparator}");
            Console.Write($@"{Tab}{Tab}{Quote}reviews{Quote}{KeyValueSeparator} {ArrayStart}");

            if (product.Reviews is null)
            {
                Console.WriteLine(ArrayEnd);
            }
            else
            {
                Console.WriteLine();

                int lastReviewIndex = product.Reviews.Count - 1;
                for (int j = 0; j <= lastReviewIndex; j++)
                {
                    string review = product.Reviews[j];
                    Console.Write($@"{Tab}{Tab}{Tab}{review}");
                    HandleLastElementEnding(j, lastReviewIndex);
                }
                
                Console.WriteLine($@"{Tab}{Tab}{ArrayEnd}");
            }
        
            Console.Write($@"{Tab}{ObjectEnd}");
            HandleLastElementEnding(i, lastProductIndex);
        }
        
        Console.WriteLine(ArrayEnd);
    }
    
    /// <summary>
    /// Updates current node with new node.
    /// Setting new node if it <see cref="DictionaryNode"/> or <see cref="ListNode"/>
    /// or adds node to current node.
    /// </summary>
    /// <param name="currentNode">Node to update.</param>
    /// <param name="newNode">Node to insert.</param>
    /// <param name="updateCurrent">Should update current?</param>
    /// <param name="isString">Should check if string can be key in object.</param>
    /// <exception cref="Exception">Error with inserting node.</exception>
    private static void UpdateNode(ref BaseNode? currentNode, BaseNode newNode, 
        bool updateCurrent = true, bool isString = false)
    {
        switch (currentNode)
        {
            case DictionaryNode dictionaryNode when updateCurrent:
                currentNode = isString 
                    ? dictionaryNode.AddKeyOrValue(newNode) 
                    : dictionaryNode.AddWithKeyFromCache(newNode);
                break;
            case DictionaryNode dictionaryNode when isString:
                dictionaryNode.AddKeyOrValue(newNode);
                break;
            case DictionaryNode dictionaryNode:
                dictionaryNode.AddWithKeyFromCache(newNode);
                break;
            case ListNode listNode:
            {
                if (updateCurrent)
                {
                    currentNode = listNode.Add(newNode);
                }
                else
                {
                    listNode.Add(newNode);
                }

                break;
            }
            case null:
                currentNode = newNode;
                break;
            default:
                throw new Exception();
        }
    }

    /// <summary>
    /// Parses any json strings including whitespaces and breaks.
    /// </summary>
    /// <param name="json">String in JSON format.</param>
    /// <returns>Root node.</returns>
    private static BaseNode? ParseJson(string json)
    {
        var jsonSb = new StringBuilder(json.Trim());
        BaseNode? node = null;

        for (int i = 0; i < jsonSb.Length; i++)
        {
            BaseNode newNode;
            char letter = jsonSb[i];
            string value = string.Empty;

            switch (letter)
            {
                // Arrays.
                case ArrayStart:
                    var listData = new List<BaseNode>();
                    newNode = new ListNode(listData, node);
                    UpdateNode(ref node, newNode);
                    break;
                case ArrayEnd when node is ListNode:
                    if (node.Parent is null)
                    {
                        return node;
                    }
                    node = node.Parent;
                    break;
                case ArrayEnd:
                    throw new Exception();
                
                // Objects.
                case ObjectStart:
                    var dictData = new Dictionary<StringNode, BaseNode>();
                    newNode = new DictionaryNode(dictData, node);
                    UpdateNode(ref node, newNode);
                    break;
                case ObjectEnd when node is DictionaryNode:
                    if (node.Parent is null)
                    {
                        return node;
                    }
                    node = node.Parent;
                    break;
                case ObjectEnd:
                    throw new Exception();
                
                // Values.
                case Quote:
                {
                    int rightQuoteIndex = Helpers.GetRightIndex(jsonSb, i, QuoteEndings, Escapes);
                    value = jsonSb.ToString(i, rightQuoteIndex + 1 - i);
                
                    newNode = new StringNode(value);
                    UpdateNode(ref node, newNode, false, true);
                    break;
                }
                default:
                {
                    if (letter == Environment.NewLine[0])
                    {
                        value = jsonSb.ToString(i, Environment.NewLine.Length);
                    }
                    else if (letter == BooleanTrue[0])
                    {
                        value = jsonSb.ToString(i, BooleanTrue.Length);

                        if (value != BooleanTrue)
                        {
                            throw new Exception();
                        }
                
                        newNode = new BooleanNode(true, node);
                        UpdateNode(ref node, newNode, false);
                    }
                    else if (letter == BooleanFalse[0])
                    {
                        value = jsonSb.ToString(i, BooleanFalse.Length);
                    
                        if (value != BooleanFalse)
                        {
                            throw new Exception();
                        }
                
                        newNode = new BooleanNode(false, node);
                        UpdateNode(ref node, newNode, false);
                    }
                    else if (letter == NullValue[0])
                    {
                        value = jsonSb.ToString(i, NullValue.Length);
                    
                        if (value != NullValue)
                        {
                            throw new Exception();
                        }
                
                        newNode = new BooleanNode(null, node);
                        UpdateNode(ref node, newNode, false);
                    }
                    else if (char.IsDigit(letter))
                    {
                        int rightIndex = Helpers.GetRightIndex(jsonSb, i, TypeEndings);
                        value = jsonSb.ToString(i, rightIndex - i);
                
                        double number = double.Parse(value.Replace('.', ','));
                        newNode = new NumberNode(number, node);
                        UpdateNode(ref node, newNode, false);
                    }

                    break;
                }
            }
            
            // Update cursor index with length of handled value.
            if (value.Length > 0)
            {
                i += value.Length - 1;
            }
        }

        return node;
    }

    /// <summary>
    /// Converts nodes to specified in work types.
    /// </summary>
    /// <param name="node">Root node with values in data.</param>
    /// <returns>List of data specified in work.</returns>
    private static List<Product> ConvertNodeToProducts(BaseNode? node)
    {
        var list = new List<Product>();

        if (node is not ListNode listNode)
        {
            return list;
        }
        
        List<BaseNode>? data = listNode.GetData();

        if (data is null)
        {
            return list;
        }
            
        foreach (BaseNode dataNode in data)
        {
            if (dataNode is not DictionaryNode dictionaryNode) continue;
                    
            var productIdNode = dictionaryNode["product_id"] as NumberNode;
            var productName = dictionaryNode["product_name"] as StringNode;
            var category = dictionaryNode["category"] as StringNode;
            var price = dictionaryNode["price"] as NumberNode;
            var quantityInStock = dictionaryNode["quantity_in_stock"] as NumberNode;
            var isDiscounted = dictionaryNode["is_discounted"] as BooleanNode;
                        
            var reviews = dictionaryNode["reviews"] as ListNode;
            var reviewsList = new List<string>();
            List<BaseNode>? reviewsData = reviews?.GetData();

            if (reviewsData != null)
            {
                foreach (BaseNode childNode in reviewsData)
                {
                    if (childNode is StringNode stringNode)
                    {
                        reviewsList.Add(stringNode.ToString());
                    }
                }
            }

            list.Add(new Product
            {
                Id = (int)Math.Round((double)(productIdNode?.Data ?? 0)),
                Name = productName?.ToString(),
                Category = category?.ToString(),
                Price = (double)(price?.Data ?? 0),
                QuantityInStock = (int)Math.Round((double)(quantityInStock?.Data ?? 0)),
                IsDiscounted = (bool)(isDiscounted?.Data ?? false),
                Reviews = reviewsList,
            });
        }

        return list;
    }

    /// <summary>
    /// Include work of two methods <see cref="ParseJson"/> and <see cref="ConvertNodeToProducts"/>.
    /// Firstly it parse string to nodes, then it converts nodes to Array of Products.
    /// </summary>
    /// <returns>Array of products.</returns>
    public static List<Product> ReadJson()
    {
        var sbText = new StringBuilder();
        while (Console.ReadLine() is { } line)
        {
            sbText.Append(line);
        }
            
        string text = sbText.ToString();
        BaseNode? rootNode = ParseJson(text);
        
        return ConvertNodeToProducts(rootNode);
    }
}