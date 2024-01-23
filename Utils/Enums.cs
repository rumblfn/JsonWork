namespace Utils;

public enum Color
{
    Condition,
    Secondary,
    Primary,
    Error,
}

public enum ModelAction
{
    Filter,
    Sort
}

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
    
    ShowData,
    SaveToExistingFile,
    SaveToNewFile,
}
