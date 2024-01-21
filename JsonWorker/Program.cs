using System.Reflection;
using Utils;

namespace JsonWorker;

/// <summary>
/// Main class of the program.
/// </summary>
public static class Program
{
    private const ConsoleKey ExitKey = ConsoleKey.Q;
    
    /// <summary>
    /// Checks for exit from the program.
    /// </summary>
    /// <returns>Key is not <see cref="ExitKey"/>.</returns>
    private static bool HandleAgain()
    {
        ConsoleMethod.NicePrint(MessageHelper.Get("Again", "KEY", ExitKey.ToString()));
        return ConsoleMethod.ReadKey() != ExitKey;
    }
    
    /// <summary>
    /// Entry point of the program.
    /// </summary>
    private static void Main()
    {
        ConsoleMethod.NicePrint(MessageHelper.Get("ProgramStarted"));
        
        do
        {
            try
            {
                JsonProcess.Run();
            }
            catch (Exception ex)
            {
                ConsoleMethod.NicePrint(ex.Message, Color.Error);
            }
        } while (HandleAgain());
        
        ConsoleMethod.NicePrint(MessageHelper.Get("ProgramFinished"));
    }
}
