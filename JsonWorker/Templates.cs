using JsonWorker.Components;
using Utils;

namespace JsonWorker;

/// <summary>
/// Use for menu templates.
/// </summary>
public static class Templates
{
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
                new(MessageHelper.Get("ShowData"), ActionType.ShowData),
                new(MessageHelper.Get("Save"), ActionType.SaveToExistingFile),
                new(MessageHelper.Get("SaveNew"), ActionType.SaveToNewFile),
            }),
        };
    }
    
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
    
    public static MenuGroup[] SortByStringItems(string sortKey)
    {
        return new MenuGroup [] {
            new (MessageHelper.Get("SortType", "KEY", sortKey), new MenuItem[]
            {
                new("Alphabetical", ActionType.SortByNameAlphabetical),
                new("Reverse", ActionType.SortByNameAlphabeticalReverse),
            }),
        };
    }
    
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