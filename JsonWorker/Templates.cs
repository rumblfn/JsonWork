using JsonWorker.Components;
using Utils;

namespace JsonWorker;

/// <summary>
/// Use for menu templates.
/// </summary>
public static class Templates
{
    public static readonly MenuGroup[] InputTypeGroup = {
        new ("Select input type", new MenuItem[]
        {
            new(Maps.GetInputType(InputType.Console), () =>
            {
                ConsoleMethod.NicePrint(MessageHelper.Get("ConsoleInput"), Color.Condition, Environment.NewLine);
                string json = ConsoleMethod.ReadLine();
            }, true),
            new(Maps.GetInputType(InputType.File), () =>
            {
                
            }, false),
        }),
    };
}