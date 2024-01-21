using Utils;

namespace JsonWorker.Components;

/// <summary>
/// Menu item to select.
/// </summary>
public class MenuItem
{
    private string Name { get; }
    public Action SelectAction;
    public bool Selected;

    /// <summary>
    /// Initialization.
    /// </summary>
    /// <param name="name">Element name.</param>
    /// <param name="selectAction">The method being called.</param>
    /// <param name="selected">Is item selected.</param>
    public MenuItem(string name, Action selectAction, bool selected)
    {
        Name = name;
        Selected = selected;
        SelectAction = selectAction;
    }

    /// <summary>
    /// Responsible for the output of the menu item.
    /// </summary>
    public void Write()
    {
        ConsoleMethod.NicePrint(" -" + Name + ";", 
            Selected ? Color.Condition : Color.Secondary, "");
    }
}