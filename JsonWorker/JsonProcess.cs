using JsonLib;

namespace JsonWorker;

/// <summary>
/// Processing solution cycle.
/// </summary>
public static class JsonProcess
{
    /// <summary>
    /// Program cycle.
    /// </summary>
    public static void Run()
    {
        JsonParser.ReadJson("/Users/samilvaliahmetov/Projects/ControlHomework3-1/assets/data_4V.json");
        
        // var dp = new DataPanel(Templates.InputTypeGroup, false);
        // dp.Run();
    }
}