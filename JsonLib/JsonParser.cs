using System.Text;
using JsonLib.Nodes;
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
    private const char ElementsSeparator = ',';

    private const char Quote = '"';
    private const char QuoteEscape = '\\';

    private const string NullValue = "null";
    private const string BooleanTrue = "true";
    private const string BooleanFalse = "false";

    private static readonly char[] QuoteEndings = { Quote };
    private static readonly char[] Escapes = { QuoteEscape };
    private static readonly char[] TypeEndings = { ElementsSeparator, ArrayEnd, ObjectEnd, WhiteSpace };
    
    /// <summary>
    /// Method for parsing json in one line.
    /// </summary>
    public static void WriteJson()
    {
        
    }
    
    /// <summary>
    /// Updates node.
    /// </summary>
    /// <param name="currentNode">Node to update.</param>
    /// <param name="newNode">Node to insert.</param>
    /// <param name="updateCurrent">Should update current?</param>
    /// <param name="isString">Should check if string can be key in object.</param>
    /// <exception cref="Exception">Error with inserting node.</exception>
    private static void UpdateNode(ref BaseNode? currentNode, BaseNode newNode, bool updateCurrent = true, bool isString = false)
    {
        switch (currentNode)
        {
            case DictionaryNode dictionaryNode when updateCurrent:
                currentNode = isString ? dictionaryNode.AddKeyOrValue(newNode) : dictionaryNode.AddWithKeyFromCache(newNode);
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
    private static void ParseJson(string json)
    {
        var jsonSb = new StringBuilder(json.Trim());
        BaseNode? node = null;

        for (int i = 0; i < jsonSb.Length - 1; i++)
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
                
                        decimal number = decimal.Parse(value.Replace('.', ','));
                        newNode = new NumberNode(number, node);
                        UpdateNode(ref node, newNode, false);
                    }

                    break;
                }
            }
            
            // Update cursor index with length of handled value.
            i += value.Length;
        }
        
        Console.WriteLine(node);
    }

    /// <summary>
    /// Parses string to json.
    /// </summary>
    /// <param name="path">Path to json format file.</param>
    public static void ReadJson(string path)
    {
        var oneLine = new StringBuilder();
        using var reader = new StreamReader(path);
        while (reader.ReadLine() is { } line)
        {
            oneLine.Append(line.Trim());
        }
        ParseJson(oneLine.ToString());
    }
}