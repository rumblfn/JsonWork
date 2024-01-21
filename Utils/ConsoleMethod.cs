namespace Utils;

/// <summary>
/// Class for common console methods.
/// </summary>
public static class ConsoleMethod
{
    /// <summary>
    /// Simple readKey method to avoid errors.
    /// </summary>
    /// <param name="intercept">Not display input?</param>
    /// <returns>Read key.</returns>
    public static ConsoleKey ReadKey(bool intercept = true)
    {
        try
        {
            return Console.ReadKey(intercept).Key;
        }
        catch (InvalidOperationException ex)
        {
            var key = ConsoleKey.Spacebar;
            NicePrint(MessageHelper.Get(
                "InvalidOperationException", "KEY", key.ToString(), "EXCEPTION", ex.Message));

            return key;
        }
    }

    /// <summary>
    /// Simple readLine method to avoid errors.
    /// </summary>
    /// <returns>Read or empty string.</returns>
    public static string ReadLine()
    {
        try
        {
            return Console.ReadLine() ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Prints a message with a specified color and a specified line end.
    /// </summary>
    /// <param name="message">Message content.</param>
    /// <param name="color">Message color.</param>
    /// <param name="end">End of line.</param>
    public static void NicePrint(string? message, Color color = Color.Secondary, string? end = null)
    {
        Console.ForegroundColor = Maps.GetColor(color);
        Console.Write((message ?? "") + (end ?? Environment.NewLine));
        Console.ResetColor();
    }
}