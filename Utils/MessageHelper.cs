using System.Reflection;
using System.Resources;

namespace Utils;

/// <summary>
/// Localization for program messages.
/// </summary>
public static class MessageHelper
{
    private static readonly ResourceManager Rm;

    /// <summary>
    /// Sets default .resx file with default language.
    /// </summary>
    static MessageHelper()
    {
        Rm = new ResourceManager("Utils.Languages.messages", Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Gets message from .resx file.
    /// Specify message arguments after name param: key, value ...
    /// </summary>
    /// <param name="name">Message key.</param>
    /// <param name="arguments">Message arguments.</param>
    /// <returns>Message value.</returns>
    public static string Get(string name, params string[] arguments)
    {
        string message = Rm.GetString(name) ?? "";
        
        for (int i = 0; i < arguments.Length; i += 2)
        {
            message = message.Replace($"{{{arguments[i]}}}", arguments[i + 1]);
        }
        
        return message;
    }
}