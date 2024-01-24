using JsonWorker.Components;
using Utils;

namespace JsonWorker;

/// <summary>
/// Provides menu panels for working with program,
/// </summary>
public static class Templates
{
    /// <summary>
    /// Panel for initializing data.
    /// It provides console and file actions.
    /// </summary>
    /// <returns>Input panel.</returns>
    public static MenuGroup[] InputTypeItems()
    {
        return new [] {
            new MenuGroup(MessageHelper.Get("InputPanelName"), new MenuItem[]
            {
                new(MessageHelper.Get("InputTypeConsole"), ActionType.ConsoleInput),
                new(MessageHelper.Get("InputTypeFile"), ActionType.FileInput),
            })
        };
    }
    
    /// <summary>
    /// Main panel for processing data.
    /// It provides sorts, filters and data managing actions.
    /// </summary>
    /// <returns>Work panel.</returns>
    public static MenuGroup[] WorkTypeItems()
    {
        return new MenuGroup [] {
            new (MessageHelper.Get("FilterAction"), new MenuItem[]
            {
                new(MessageHelper.Get("ProductId"), ActionType.FilterById),
                new(MessageHelper.Get("ProductName"), ActionType.FilterByName),
                new(MessageHelper.Get("ProductCategory"), ActionType.FilterByCategory),
                new(MessageHelper.Get("ProductPrice"), ActionType.FilterByPrice),
                new(MessageHelper.Get("ProductQuantityInStock"), ActionType.FilterByQuantity),
                new(MessageHelper.Get("ProductIsDiscounted"), ActionType.FilterByDiscount),
                new(MessageHelper.Get("ProductReviews"), ActionType.FilterByReviews),
            }),
            new (MessageHelper.Get("SortAction"), new MenuItem[]
            {
                new(MessageHelper.Get("ProductId"), ActionType.SortById),
                new(MessageHelper.Get("ProductName"), ActionType.SortByName),
                new(MessageHelper.Get("ProductCategory"), ActionType.SortByCategory),
                new(MessageHelper.Get("ProductPrice"), ActionType.SortByPrice),
                new(MessageHelper.Get("ProductQuantityInStock"), ActionType.SortByQuantity),
                new(MessageHelper.Get("ProductReviews"), ActionType.SortByReviews),
            }),
            new (MessageHelper.Get("DataAction"), new MenuItem[]
            {
                new(MessageHelper.Get("SetInitialData"), ActionType.SetInitialData),
                new(MessageHelper.Get("ShowData"), ActionType.ShowData),
                new(MessageHelper.Get("Save"), ActionType.SaveToExistingFile),
                new(MessageHelper.Get("SaveNew"), ActionType.SaveToNewFile),
            }),
        };
    }
    
    /// <summary>
    /// Panel for selecting sort type for numbers
    /// specified by their actions and fields.
    /// </summary>
    /// <param name="sortKey">Field in Model.</param>
    /// <param name="firstType">Ascending sort type.</param>
    /// <param name="secondType">Descending sort type.</param>
    /// <returns>Panel for selecting sort type for numbers.</returns>
    public static MenuGroup[] SortByNumberItems(string sortKey, ActionType firstType, ActionType secondType)
    {
        return new MenuGroup [] {
            new (MessageHelper.Get("SortType", "KEY", sortKey), new MenuItem[]
            {
                new("Ascending", firstType),
                new("Descending", secondType),
            }),
        };
    }
    
    /// <summary>
    /// Panel for selecting sort type for strings
    /// specified by their actions and fields.
    /// </summary>
    /// <param name="sortKey">Field in Model.</param>
    /// <param name="firstType">Alphabetical sort type.</param>
    /// <param name="secondType">Alphabetical reverse sort type.</param>
    /// <returns>Panel for selecting sort type for strings.</returns>
    public static MenuGroup[] SortByStringItems(string sortKey, ActionType firstType, ActionType secondType)
    {
        return new MenuGroup [] {
            new (MessageHelper.Get("SortType", "KEY", sortKey), new MenuItem[]
            {
                new("Alphabetical", firstType),
                new("Reverse", secondType),
            }),
        };
    }
    
    /// <summary>
    /// Panel selecting discount type action.
    /// </summary>
    /// <returns>Discount type panel.</returns>
    public static MenuGroup[] DiscountTypeItems()
    {
        return new MenuGroup [] {
            new (MessageHelper.Get("DiscountTypeAction"), new MenuItem[]
            {
                new(MessageHelper.Get("SetDiscountTrue"), ActionType.SetDiscountTrue),
                new(MessageHelper.Get("SetDiscountFalse"), ActionType.SetDiscountFalse),
            }),
        };
    }
}