using JsonWorker.Components;
using Utils;

namespace JsonWorker;

/// <summary>
/// Panel menu (task manager) for working with data.
/// </summary>
internal class DataPanel
{
    private bool _toExit;
    private int _currentCursorRowIndex = Console.CursorTop;
    private int _currentCursorColumnIndex = Console.CursorLeft;
    
    private readonly MenuGroup[] _menuGroups;
    private readonly bool _repeat;

    private ConsoleKey _pressedButtonKey;
    private bool _elementSelected;

    /// <summary>
    /// Initialization.
    /// </summary>
    /// <param name="groups">Panel items.</param>
    /// <param name="repeat">If panel should be cycled.</param>
    public DataPanel(MenuGroup[] groups, bool repeat)
    {
        _repeat = repeat;
        _menuGroups = groups;
        _elementSelected = false;
        
        (int rowIndex, int columnIndex) = GetSelectedItemIndexes();
        UpdateSelectedItem(rowIndex, columnIndex);
    }

    /// <summary>
    /// It is used to prepare the canvas for work.
    /// </summary>
    /// <param name="message">Starting message.</param>
    private void Restore(string message)
    {
        Console.Clear();
        ConsoleMethod.NicePrint(message);
        UpdateCursorPosition();
    }

    /// <summary>
    /// Gets selected item indexes.
    /// </summary>
    /// <returns>Selected item row and column indexes.</returns>
    /// <exception cref="Exception">If nothing was found.</exception>
    private (int, int) GetSelectedItemIndexes()
    {
        for (int rowIndex = 0; rowIndex < _menuGroups.Length; rowIndex++)
        {
            MenuItem[] items = _menuGroups[rowIndex].Items;
            for (int columnIndex = 0; columnIndex < items.Length; columnIndex++)
            {
                MenuItem item = items[columnIndex];
                if (item.Selected)
                {
                    return (rowIndex, columnIndex);
                }
            }
        }

        throw new Exception(MessageHelper.Get("SelectedItemError"));
    }
    
    /// <summary>
    /// Updates the currently selected item.
    /// </summary>
    private void UpdateSelectedItem(int updatedRow, int updatedColumn)
    {
        (int currentRow, int currentColumn) = GetSelectedItemIndexes();
        MenuItem[] rowItems = _menuGroups[currentRow].Items;
        
        // Check accessible to change selected item.
        if (
            currentRow == updatedRow && currentColumn == updatedColumn
            || updatedColumn >= rowItems.Length || updatedColumn < 0
            || updatedRow >= _menuGroups.Length || updatedRow < 0
            )
        {
            return;
        }

        rowItems[currentColumn].Selected = false;
        _menuGroups[updatedRow].Items[updatedColumn].Selected = true;
    }
    
    /// <summary>
    /// Processes the selected item.
    /// </summary>
    private void HandleEnterKey()
    {
        _elementSelected = true;
        (int rowIndex, int columnIndex) = GetSelectedItemIndexes();
        MenuItem item = _menuGroups[rowIndex].Items[columnIndex];
        item.SelectAction.Invoke();
    }

    /// <summary>
    /// Updates the indexes of the selected group and item.
    /// </summary>
    private void HandleKeys()
    {
        (int rowIndex, int columnIndex) = GetSelectedItemIndexes();
        
        switch (_pressedButtonKey)
        {
            case ConsoleKey.DownArrow:
                rowIndex++;
                break;
            case ConsoleKey.UpArrow:
                rowIndex--;
                break;
            case ConsoleKey.LeftArrow:
                columnIndex--;
                break;
            case ConsoleKey.RightArrow:
                columnIndex++;
                break;
            case ConsoleKey.Q:
                _toExit = true;
                break;
            case ConsoleKey.Enter:
                HandleEnterKey();
                return;
            default:
                return;
        }
        
        UpdateSelectedItem(rowIndex, columnIndex);
    }
    
    /// <summary>
    /// Updates cursor position of the console.
    /// </summary>
    private void UpdateCursorPosition()
    {
        _currentCursorRowIndex = Console.CursorTop;
        _currentCursorColumnIndex = Console.CursorLeft;
    }
    
    /// <summary>
    /// Panel runner.
    /// </summary>
    public void Run()
    {
        Restore(MessageHelper.Get("PanelMessage"));

        while (!_toExit)
        {
            DrawPanel();
            _pressedButtonKey = ConsoleMethod.ReadKey();
            HandleKeys();

            if (!_repeat && _elementSelected)
            {
                return;
            }
        }
    }
    
    /// <summary>
    /// Displays the panel on the screen.
    /// </summary>
    private void DrawPanel()
    {
        Console.SetCursorPosition(_currentCursorColumnIndex, _currentCursorRowIndex);
        foreach (MenuGroup group in _menuGroups)
        {
            group.Write();
        }
    }
}