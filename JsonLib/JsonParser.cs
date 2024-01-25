using System.Globalization;
using JsonLib.Nodes;
using System.Text;

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

    private const string Tab2 = "  ";
    private const string Tab4 = "    ";

    private const char Quote = '"';
    private const char QuoteEscape = '\\';

    private const string NullValue = "null";
    private const string BooleanTrue = "true";
    private const string BooleanFalse = "false";

    private static readonly char[] QuoteEndings = { Quote };
    private static readonly char[] Escapes = { QuoteEscape };
    private static readonly char[] TypeEndings = { ElementsSeparator, ArrayEnd, ObjectEnd, WhiteSpace };
    
    private static BaseNode? _node;
    private static int _currentIndex;
    private static char _currentLetter;
    private static StringBuilder _jsonSb = new();
    
    /// <summary>
    /// Use for writing dictionary key value pairs with formatting.
    /// </summary>
    /// <param name="key">Key in pair.</param>
    /// <param name="value">Value in pair.</param>
    private static void WriteKeyValuePair(string key, string? value)
    {
        Console.WriteLine($@"{Tab4}{Quote}{key}{Quote}{KeyValueSeparator} {value}{ElementsSeparator}");
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
            Console.WriteLine($@"{Tab2}{ObjectStart}");
            
            WriteKeyValuePair("product_id", product.Id.ToString());
            WriteKeyValuePair("product_name", product.Name);
            WriteKeyValuePair("category", product.Category);
            WriteKeyValuePair("price", product.Price.ToString(CultureInfo.InvariantCulture));
            WriteKeyValuePair("quantity_in_stock", product.QuantityInStock.ToString());

            Console.WriteLine(product.IsDiscounted
                ? $@"{Tab4}{Quote}is_discounted{Quote}{KeyValueSeparator} {BooleanTrue}{ElementsSeparator}"
                : $@"{Tab4}{Quote}is_discounted{Quote}{KeyValueSeparator} {BooleanFalse}{ElementsSeparator}");
            Console.Write($@"{Tab4}{Quote}reviews{Quote}{KeyValueSeparator} {ArrayStart}");

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
                    Console.Write($@"{Tab2}{Tab4}{review}");
                    HandleLastElementEnding(j, lastReviewIndex);
                }
                
                Console.WriteLine($@"{Tab4}{ArrayEnd}");
            }
        
            Console.Write($@"{Tab2}{ObjectEnd}");
            HandleLastElementEnding(i, lastProductIndex);
        }
        
        Console.WriteLine(ArrayEnd);
    }
    
    /// <summary>
    /// Updates current node with new node.
    /// Setting new node if it <see cref="DictionaryNode"/> or <see cref="ListNode"/>
    /// or adds node to current node.
    /// </summary>
    /// <param name="newNode">Node to insert.</param>
    /// <exception cref="Exception">Error with inserting node.</exception>
    private static void UpdateNode(BaseNode newNode)
    {
        _node = _node switch
        {
            DictionaryNode dictionaryNode => dictionaryNode.Add(newNode),
            ListNode listNode => listNode.Add(newNode),
            null => newNode,
            _ => throw new Exception(),
        };
    }

    /// <summary>
    /// Validate value with target value.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <param name="validValue">Target value.</param>
    /// <exception cref="Exception">Different values.</exception>
    private static void ValidateConstantValue(string value, string validValue)
    {
        if (value != validValue)
        {
            throw new ArgumentException($"Value {value} is not {validValue}");
        }
    }

    /// <summary>
    /// Ups node level.
    /// </summary>
    private static void SwitchToParent()
    {
        if (_node?.Parent is null)
        {
            return;
        }
        
        _node = _node.Parent;
    }

    /// <summary>
    /// Parses arrays and objects in json format.
    /// </summary>
    /// <exception cref="Exception">If incorrect structure ending.</exception>
    private static void ParseIndependentStructures()
    {
        switch (_currentLetter)
        {
            case ArrayStart:
                UpdateNode(new ListNode(_node));
                break;
            case ObjectStart:
                UpdateNode(new DictionaryNode(_node));
                break;
            case ArrayEnd when _node is ListNode:
            case ObjectEnd when _node is DictionaryNode:
                SwitchToParent();
                break;
            case ArrayEnd:
            case ObjectEnd:
                throw new FormatException("Structure ending incorrect.");
        }
    }

    /// <summary>
    /// Parses value specified by current letter.
    /// </summary>
    /// <returns>Parsed value.</returns>
    private static string ParseValue()
    {
        return _currentLetter switch
        {
            _ when _currentLetter == NullValue[0] => _jsonSb.ToString(_currentIndex, NullValue.Length),
            _ when _currentLetter == BooleanTrue[0] => _jsonSb.ToString(_currentIndex, BooleanTrue.Length),
            _ when _currentLetter == BooleanFalse[0] => _jsonSb.ToString(_currentIndex, BooleanFalse.Length),
            Quote => _jsonSb.ToString(_currentIndex, GetRightIndex(QuoteEndings, Escapes) - _currentIndex + 1),
            _ when _currentLetter == Environment.NewLine[0] => 
                _jsonSb.ToString(_currentIndex, Environment.NewLine.Length),
            _ when char.IsDigit(_currentLetter) => 
                _jsonSb.ToString(_currentIndex, GetRightIndex(TypeEndings) - _currentIndex),
            _ => string.Empty
        };
    }
    
    /// <summary>
    /// Parses strings, boolean, numbers and nullable values.
    /// </summary>
    private static void ParseDependentStructures()
    {
        string value = ParseValue();
        
        if (_currentLetter == Quote)
        {
            UpdateNode(new StringNode(value));
        }
        else if (_currentLetter == BooleanTrue[0])
        {
            ValidateConstantValue(value, BooleanTrue);
            UpdateNode(new BooleanNode(true, _node));
        }
        else if (_currentLetter == BooleanFalse[0])
        {
            ValidateConstantValue(value, BooleanFalse);
            UpdateNode(new BooleanNode(false, _node));
        }
        else if (_currentLetter == NullValue[0])
        {
            ValidateConstantValue(value, NullValue);
            UpdateNode(new BooleanNode(null, _node));
        }
        else if (char.IsDigit(_currentLetter))
        {
            double number = double.Parse(value.Replace('.', ','));
            UpdateNode(new NumberNode(number, _node));
        }
        
        // Update cursor index with length of handled value.
        if (value.Length > 0)
        {
            _currentIndex += value.Length - 1;
        }
    }
    
    /// <summary>
    /// It iterates through the StringBuilder starting from the left index,
    /// while skipping any escape characters if specified.
    /// </summary>
    /// <param name="endings">Array of ending characters.</param>
    /// <param name="escape">Optional array of escape characters.</param>
    /// <returns>Right index of the first occurrence of an ending character.</returns>
    /// <exception cref="Exception">If no ending character is found, an exception is thrown.</exception>
    private static int GetRightIndex(char[] endings, char[]? escape = null)
    {
        for (int i = _currentIndex + 1; i < _jsonSb.Length; i++)
        {
            if (
                !endings.Contains(_jsonSb[i])
                || escape is not null && escape.Contains(_jsonSb[i - 1]))
            {
                continue;
            }
            
            return i;
        }

        throw new Exception();
    }

    /// <summary>
    /// Parses any json strings including whitespaces and breaks.
    /// </summary>
    /// <returns>Root node.</returns>
    private static void ParseJson()
    {
        for (_currentIndex = 0; _currentIndex < _jsonSb.Length; _currentIndex++)
        {
            _currentLetter = _jsonSb[_currentIndex];
            ParseIndependentStructures();
            ParseDependentStructures();
        }
    }

    /// <summary>
    /// Converts nodes to specified in work types.
    /// </summary>
    /// <returns>List of data specified in work.</returns>
    private static List<Product> ConvertNodeToProducts()
    {
        var list = new List<Product>();

        if (
            _node is not ListNode listNode
            || listNode.GetData() is not { } data)
        {
            return list;
        }
            
        foreach (BaseNode dataNode in data)
        {
            if (dataNode is not DictionaryNode dictionaryNode)
            {
                continue;
            }
                    
            var productIdNode = dictionaryNode["product_id"] as NumberNode;
            var productName = dictionaryNode["product_name"] as StringNode;
            var category = dictionaryNode["category"] as StringNode;
            var price = dictionaryNode["price"] as NumberNode;
            var quantityInStock = dictionaryNode["quantity_in_stock"] as NumberNode;
            var isDiscounted = dictionaryNode["is_discounted"] as BooleanNode;
                        
            var reviews = dictionaryNode["reviews"] as ListNode;
            List<BaseNode>? reviewsData = reviews?.GetData();

            var product = new Product
            {
                Id = (int)Math.Round((double)(productIdNode?.Data ?? 0)),
                Name = productName?.ToString(),
                Category = category?.ToString(),
                Price = (double)(price?.Data ?? 0),
                QuantityInStock = (int)Math.Round((double)(quantityInStock?.Data ?? 0)),
                IsDiscounted = (bool)(isDiscounted?.Data ?? false),
                Reviews = new List<string>(),
            };
            list.Add(product);

            if (reviewsData == null)
            {
                continue;
            }
            
            foreach (BaseNode childNode in reviewsData)
            {
                if (childNode is StringNode stringNode)
                {
                    product.Reviews.Add(stringNode.ToString());
                }
            }
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
        // Setting default values.
        _node = null;
        _currentIndex = 0;
        _jsonSb = new StringBuilder();
        
        while (Console.ReadLine() is { } line)
        {
            _jsonSb.Append(line);
        }

        ParseJson();
        return ConvertNodeToProducts();
    }
}