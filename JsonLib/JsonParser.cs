using System.Globalization;
using System.Text;

namespace JsonLib;

/// <summary>
/// Node with dynamic type.
/// </summary>
public class DynamicJsonNode
{
    public dynamic? Node { get; set; }
    public dynamic? Parent { get; }
    
    public bool IsEnded { get; set; }

    public DynamicJsonNode(dynamic? node = null, dynamic? parent = null)
    {
        Node = node;
        Parent = parent;
    }
}

/// <summary>
/// Class for reading/writing json.
/// </summary>
public static class JsonParser
{
    private const char ArrayEnd = ']';
    private const char ArrayStart = '[';
    
    private const char ObjectEnd = '}';
    private const char ObjectStart = '{';

    private const char Empty = ' ';
    private const char Quote = '"';
    private const char Point = '.';
    private const char Escape = '\\';
    private const char ElementsSeparator = ',';
    private const char KeyValueSeparator = ':';

    private static readonly char[] Digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

    private const string NullValue = "null";
    private const string BooleanTrue = "true";
    private const string BooleanFalse = "false";
    
    /// <summary>
    /// Converts json string to one line.
    /// </summary>
    /// <param name="json">Json string.</param>
    /// <returns>Json in one line.</returns>
    private static string ConvertToOneLine(string json)
    {
        return string.Join("", json.Split(Environment.NewLine));
    }
    
    /// <summary>
    /// Method for parsing json in one line.
    /// </summary>
    public static void WriteJson()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="line"></param>
    private static void ParseLine(string line)
    {
        var lineSb = new StringBuilder(line);
        var node = new DynamicJsonNode();

        for (int i = 0; i < lineSb.Length; i++)
        {
            char letter = lineSb[i];

            if (node.Node is List<dynamic>)
            {
                if (letter is KeyValueSeparator or ObjectEnd)
                {
                    throw new Exception();
                }
            }
            else if (node.Node is Dictionary<string, dynamic>)
            {
                
            }
            else
            {
                
            }
        }
        
        // var keyOrValue = new StringBuilder();
        // var current = new DynamicJsonNode();
        //
        // bool isQuoted = false;
        // bool isQuotedEnd = false;
        //
        // bool separated = false;
        //
        // bool isPointed = false;
        //
        // var lineSb = new StringBuilder(line);
        // for (int i = 0; i < lineSb.Length; i++)
        // {
        //     DynamicJsonNode? node = null;
        //     char letter = lineSb[i];
        //     switch (letter)
        //     {
        //         case ObjectStart:
        //             if (isQuoted)
        //             {
        //                 keyOrValue.Append(letter);
        //             }
        //             else
        //             {
        //                 node = new DynamicJsonNode(new Dictionary<string, dynamic?>(), current);
        //                 current = node;
        //             }
        //             break;
        //         case ObjectEnd:
        //             current.IsEnded = true;
        //             break;
        //         case ArrayStart:
        //             node = new DynamicJsonNode(new List<dynamic>(), current);
        //             current = node;
        //             break;
        //         case ArrayEnd:
        //             current.IsEnded = true;
        //             break;
        //         case Quote:
        //             if (node is null)
        //             {
        //                 node = new DynamicJsonNode(string.Empty, current);
        //                 current = node;
        //             }
        //             
        //             if (isQuoted)
        //             {
        //                 isQuotedEnd = true;
        //                 separated = false;
        //                 if (node.Node is List<dynamic>)
        //                 {
        //                     node.Node.Add(keyOrValue);
        //                 }
        //                 else if (node.Node is Dictionary<string, dynamic>)
        //                 {
        //                     node.Node[keyOrValue] = null;
        //                 }
        //                 else if ('"' + keyOrValue.ToString() + '"' == lineSb.ToString())
        //                 {
        //                     node.Node = keyOrValue.ToString();
        //                 }
        //                 else
        //                 {
        //                     throw new Exception();
        //                 }
        //             }
        //             else
        //             {
        //                 if (keyOrValue.Length > 0)
        //                 {
        //                     throw new Exception();
        //                 }
        //                 isQuoted = true;
        //             }
        //
        //             break;
        //         case Escape:
        //             if (isQuoted)
        //             {
        //                 keyOrValue.Append(lineSb[i + 1]);
        //                 i++;
        //             }
        //             else
        //             {
        //                 throw new Exception();
        //             }
        //
        //             break;
        //         case KeyValueSeparator:
        //             if (isQuotedEnd)
        //             {
        //                 if (separated || node?.Node is List<dynamic>)
        //                 {
        //                     throw new Exception();
        //                 }
        //                 separated = true;
        //             }
        //             else
        //             {
        //                 keyOrValue.Append(letter);
        //             }
        //             break;
        //         case ElementsSeparator:
        //             
        //             break;
        //         case Empty:
        //             if (isQuoted)
        //             {
        //                 keyOrValue.Append(letter);
        //             } 
        //             // else if (keyOrValue.Length > 0)
        //             // {
        //             //     string compareString = keyOrValue.ToString();
        //             //     if (compareString == NullValue)
        //             //     {
        //             //         
        //             //     } else if (compareString == BooleanTrue)
        //             //     {
        //             //         
        //             //     } else if (compareString == BooleanFalse)
        //             //     {
        //             //         
        //             //     } else if (isPointed)
        //             //     {
        //             //         
        //             //     }
        //             //     else
        //             //     {
        //             //         throw new Exception();
        //             //     }
        //             // }
        //
        //             break;
        //         default:
        //             if (letter == Point && !isQuoted)
        //             {
        //                 if (isPointed || !Digits.Contains(lineSb[i + 1]))
        //                 {
        //                     throw new Exception();
        //                 }
        //                 
        //                 isPointed = true;
        //             }
        //             keyOrValue.Append(letter);
        //             break;
        //     }
        // }
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
        ParseLine(oneLine.ToString());
    }
}