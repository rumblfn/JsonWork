namespace Utils;

/// <summary>
/// Console colors for mapping.
/// </summary>
public enum Color
{
    Condition,
    Secondary,
    Primary,
    Error,
}

/// <summary>
/// Panel actions: input, filter, sorting, processing types.
/// </summary>
public enum ActionType
{
    FileInput,
    ConsoleInput,
    
    FilterById,
    FilterByName,
    FilterByPrice,
    FilterByReviews,
    FilterByCategory,
    FilterByQuantity,
    FilterByDiscount,
    
    SortById,
    SortByName,
    SortByPrice,
    SortByReviews,
    SortByCategory,
    SortByQuantity,
    
    SetDiscountTrue,
    SetDiscountFalse,
    
    SortByIdAscending,
    SortByIdDescending,
    SortByNameAlphabetical,
    SortByNameAlphabeticalReverse,
    SortByPriceAscending,
    SortByPriceDescending,
    SortByReviewsAscending,
    SortByReviewsDescending,
    SortByCategoryAscending,
    SortByCategoryDescending,
    SortByQuantityAscending,
    SortByQuantityDescending,
    
    SetInitialData,
    ShowData,
    SaveToExistingFile,
    SaveToNewFile,
}
