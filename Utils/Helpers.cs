using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Utils;

/// <summary>
/// Provides a set of helper functions.
/// </summary>
public static class Helpers
{
    /// <summary>
    /// It iterates through the StringBuilder starting from the left index,
    /// while skipping any escape characters if specified.
    /// </summary>
    /// <param name="sb">StringBuilder object.</param>
    /// <param name="leftIndex">Start index.</param>
    /// <param name="endings">Array of ending characters.</param>
    /// <param name="escape">Optional array of escape characters.</param>
    /// <returns>Right index of the first occurrence of an ending character.</returns>
    /// <exception cref="Exception">If no ending character is found, an exception is thrown.</exception>
    public static int GetRightIndex(StringBuilder sb, int leftIndex, char[] endings, char[]? escape = null)
    {
        for (int i = leftIndex + 1; i < sb.Length; i++)
        {
            if (!endings.Contains(sb[i]))
            {
                continue;
            }

            if (escape is not null && escape.Contains(sb[i - 1]))
            {
                continue;
            }
            
            return i;
        }

        throw new Exception();
    }
}