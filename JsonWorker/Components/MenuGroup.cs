using Utils;

namespace JsonWorker.Components;

/// <summary>
/// Menu item to select.
/// </summary>
public class MenuGroup
{
    private string Name { get; }
    public MenuItem[] Items { get; }

    /// <summary>
    /// Initialization.
    /// </summary>
    /// <param name="name">Group name.</param>
    /// <param name="items">Items in Group.</param>
    public MenuGroup(string name, MenuItem[] items)
    {
        Name = name;
        Items = items;
    }
    
    /// <summary>
    /// Responsible for the output of the menu group.
    /// </summary>
    public void Write()
    {
        if (Items.Any(item => item.Selected))
        {
            ConsoleMethod.NicePrint("?", Color.Primary, " ");
        }

        Console.Write(Name + ":");
        foreach (MenuItem item in Items)
        {
            item.Write();
        }
        Console.WriteLine("  ");
    }
}