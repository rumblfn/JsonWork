using System.Text;

namespace Utils;

/// <summary>
/// Helper functions.
/// </summary>
public static class Helpers
{
    /// <summary>
    /// Function that getting right index of specified elements and skipping specified escape elements.
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="leftIndex"></param>
    /// <param name="endings"></param>
    /// <param name="escape"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
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