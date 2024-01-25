using JsonWorker.Components;
using System.Security;
using JsonLib;
using Utils;

namespace JsonWorker;

/// <summary>
/// Processing solution cycle.
/// </summary>
public class DataManager
{
    private string _path = string.Empty;
    private List<Product> _initialProducts = new();
    private List<Product> _products = new();
    
    private List<Product> Products
    {
        set
        {
            _products = value;
            _initialProducts = new List<Product>(value);
        }
        get => _products;
    }

    private string _panelTitle = MessageHelper.Get("PanelMessage");
    private MenuGroup[] _groups = Templates.InputTypeItems();
    
    /// <summary>
    /// Program cycle.
    /// </summary>
    public void Run()
    {
        do
        {
            var dp = new DataPanel(_groups);
            MenuItem? inputTypeItem = dp.Run(_panelTitle);

            if (inputTypeItem != null)
                HandleAction(inputTypeItem.Action);
            else
                break;
        } while (true);
    }
    
    /// <summary>
    /// Handles selected action.
    /// </summary>
    /// <param name="action">Action to handle.</param>
    /// <exception cref="ArgumentOutOfRangeException">Unhandled action.</exception>
    private void HandleAction(ActionType action)
    {
        switch (action)
        {
            // Data initialization.
            case ActionType.ConsoleInput:
                HandleConsoleInput();
                UpdatePanelTitle();
                _groups = Templates.WorkTypeItems();
                break;
            case ActionType.FileInput:
                HandleFileInput();
                UpdatePanelTitle();
                _groups = Templates.WorkTypeItems();
                break;
            
            // Filters.
            case ActionType.FilterById:
                HandleFilterById();
                break;
            case ActionType.FilterByName:
                HandleFilterByName();
                break;
            case ActionType.FilterByPrice:
                HandleFilterByPrice();
                break;
            case ActionType.FilterByReviews:
                HandleFilterByReviews();
                break;
            case ActionType.FilterByCategory:
                HandleFilterByCategory();
                break;
            case ActionType.FilterByQuantity:
                HandleFilterByQuantity();
                break;
            case ActionType.FilterByDiscount:
                _panelTitle = MessageHelper.Get("DiscountTypeAction");
                _groups = Templates.DiscountTypeItems();
                break;
            case ActionType.SetDiscountTrue:
                ConsoleMethod.NicePrint(MessageHelper.Get("DiscountUpdateMessage", 
                    "STATUS", "On discount"));
                _products = _products.Where(product => product.IsDiscounted).ToList();
                _groups = Templates.WorkTypeItems();
                UpdatePanelTitle();
                break;
            case ActionType.SetDiscountFalse:
                ConsoleMethod.NicePrint(MessageHelper.Get("DiscountUpdateMessage", 
                    "STATUS", "All"));
                _products = _products.Where(product => !product.IsDiscounted).ToList();
                _groups = Templates.WorkTypeItems();
                UpdatePanelTitle();
                break;
            
            // Sort panels.
            case ActionType.SortById:
                _groups = Templates.SortByNumberItems("Id", 
                    ActionType.SortByIdAscending, ActionType.SortByIdDescending);
                break;
            case ActionType.SortByName:
                _groups = Templates.SortByStringItems("Name", 
                    ActionType.SortByNameAlphabetical, ActionType.SortByNameAlphabeticalReverse);
                break;
            case ActionType.SortByPrice:
                _groups = Templates.SortByNumberItems("Price", 
                    ActionType.SortByPriceAscending, ActionType.SortByIdDescending);
                break;
            case ActionType.SortByReviews:
                _groups = Templates.SortByNumberItems("Reviews count", 
                    ActionType.SortByReviewsAscending, ActionType.SortByReviewsDescending);
                break;
            case ActionType.SortByCategory:
                _groups = Templates.SortByNumberItems("Category", 
                    ActionType.SortByCategoryAscending, ActionType.SortByCategoryDescending);
                break;
            case ActionType.SortByQuantity:
                _groups = Templates.SortByNumberItems("Product quantity", 
                    ActionType.SortByQuantityAscending, ActionType.SortByQuantityDescending);
                break;
            
            // Sorting data.
            case ActionType.SortByIdAscending:
                _products = _products.OrderBy(product => product.Id).ToList();
                _groups = Templates.WorkTypeItems();
                break;
            case ActionType.SortByIdDescending:
                _products = _products.OrderByDescending(product => product.Id).ToList();
                _groups = Templates.WorkTypeItems();
                break;
            case ActionType.SortByNameAlphabetical:
                _products = _products.OrderBy(product => product.Name).ToList();
                _groups = Templates.WorkTypeItems();
                break;
            case ActionType.SortByNameAlphabeticalReverse:
                _products = _products.OrderByDescending(product => product.Name).ToList();
                _groups = Templates.WorkTypeItems();
                break;
            case ActionType.SortByPriceAscending:
                _products = _products.OrderBy(product => product.Price).ToList();
                _groups = Templates.WorkTypeItems();
                break;
            case ActionType.SortByPriceDescending:
                _products = _products.OrderByDescending(product => product.Price).ToList();
                _groups = Templates.WorkTypeItems();
                break;
            case ActionType.SortByReviewsAscending:
                _products = _products.OrderBy(product => product.Reviews?.Count).ToList();
                _groups = Templates.WorkTypeItems();
                break;
            case ActionType.SortByReviewsDescending:
                _products = _products.OrderByDescending(product => product.Reviews?.Count).ToList();
                _groups = Templates.WorkTypeItems();
                break;
            case ActionType.SortByCategoryAscending:
                _products = _products.OrderBy(product => product.Category).ToList();
                _groups = Templates.WorkTypeItems();
                break;
            case ActionType.SortByCategoryDescending:
                _products = _products.OrderByDescending(product => product.Category).ToList();
                _groups = Templates.WorkTypeItems();
                break;
            case ActionType.SortByQuantityAscending:
                _products = _products.OrderBy(product => product.QuantityInStock).ToList();
                _groups = Templates.WorkTypeItems();
                break;
            case ActionType.SortByQuantityDescending:
                _products = _products.OrderByDescending(product => product.QuantityInStock).ToList();
                _groups = Templates.WorkTypeItems();
                break;
            
            // Initial data.
            case ActionType.SetInitialData:
                _products = _initialProducts;
                UpdatePanelTitle();
                break;
            case ActionType.ShowData:
                HandleShow();
                break;
            case ActionType.SaveToExistingFile:
                HandlePathOutput(false);
                break;
            case ActionType.SaveToNewFile:
                HandlePathOutput(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, null);
        }
    }

    /// <summary>
    /// Provides filtering products by quantity min count.
    /// </summary>
    private void HandleFilterByQuantity()
    {
        ConsoleMethod.NicePrint(MessageHelper.Get("FilterByQuantityMessage"), 
            Color.Condition, Environment.NewLine);
                
        ConsoleMethod.NicePrint(MessageHelper.Get("FilterByMinQuantityMessage"));
        string userInput = ConsoleMethod.ReadLine();
                
        bool minQuantityParsed = int.TryParse(userInput, out int minQuantity);
        switch (minQuantityParsed)
        {
            case true:
                _products = _products.Where(product => product.QuantityInStock >= minQuantity).ToList();
                break;
            case false:
                ConsoleMethod.NicePrint(MessageHelper.Get("ParseIntegerError", 
                    "INPUT", userInput));
                break;
        }
        UpdatePanelTitle();
    }

    /// <summary>
    /// Provides filtering products by category name.
    /// </summary>
    private void HandleFilterByCategory()
    {
        ConsoleMethod.NicePrint(MessageHelper.Get("FilterByCategoryMessage"));
        string userInput = ConsoleMethod.ReadLine();
        _products = _products.Where(product => 
            product.Category != null && 
            product.Category == $"\"{userInput}\"" || product.Category == userInput).ToList();
        UpdatePanelTitle();
    }

    /// <summary>
    /// Provides filtering products by reviews count.
    /// </summary>
    private void HandleFilterByReviews()
    {
        ConsoleMethod.NicePrint(MessageHelper.Get("FilterByReviewsMessage"));
        string userInput = ConsoleMethod.ReadLine();
        _products = _products.Where(product => 
            product.Reviews != null && product.Reviews.Any(review => 
                review.Contains(userInput))).ToList();
        UpdatePanelTitle();
    }

    /// <summary>
    /// Provides filtering products by min and max price.
    /// </summary>
    private void HandleFilterByPrice()
    {
        ConsoleMethod.NicePrint(MessageHelper.Get("FilterByPriceMessage"), 
            Color.Condition, Environment.NewLine);
                
        ConsoleMethod.NicePrint(MessageHelper.Get("FilterByMinPriceMessage"));
        string userInput = ConsoleMethod.ReadLine();
                
        bool minPriceParsed = int.TryParse(userInput, out int minPrice);
        switch (minPriceParsed)
        {
            case true:
                _products = _products.Where(product => product.Price >= minPrice).ToList();
                break;
            case false:
                ConsoleMethod.NicePrint(MessageHelper.Get("ParseIntegerError", 
                    "INPUT", userInput));
                break;
        }
                
        ConsoleMethod.NicePrint(MessageHelper.Get("FilterByMaxPriceMessage"));
        userInput = ConsoleMethod.ReadLine();
                
        bool maxPriceParsed = int.TryParse(userInput, out int maxPrice);
        switch (maxPriceParsed)
        {
            case true:
                _products = _products.Where(product => product.Price <= maxPrice).ToList();
                break;
            case false:
                ConsoleMethod.NicePrint(MessageHelper.Get("ParseIntegerError", 
                    "INPUT", userInput));
                break;
        }
                
        UpdatePanelTitle();
    }

    /// <summary>
    /// Provides filtering products by name.
    /// </summary>
    private void HandleFilterByName()
    {
        ConsoleMethod.NicePrint(MessageHelper.Get("FilterByNameMessage"));
        string userInput = ConsoleMethod.ReadLine();
        _products = _products.Where(product => 
            product.Name != null && product.Name.Contains(userInput)).ToList();
        UpdatePanelTitle();
    }

    /// <summary>
    /// Provides filtering products by id.
    /// </summary>
    private void HandleFilterById()
    {
        ConsoleMethod.NicePrint(MessageHelper.Get("FilterByIdMessage"));
        string userInput = ConsoleMethod.ReadLine();
                
        bool idParsed = int.TryParse(userInput, out int searchId);
        switch (idParsed)
        {
            case true:
                _products = _products.Where(product => product.Id == searchId).ToList();
                break;
            case false:
                ConsoleMethod.NicePrint(MessageHelper.Get("ParseIntegerError", 
                    "INPUT", userInput));
                break;
        }
                
        UpdatePanelTitle();
    }

    /// <summary>
    /// Write products to standard console output.
    /// </summary>
    private void HandleShow()
    {
        JsonParser.WriteJson(_products);
        ConsoleMethod.NicePrint("Enter any key to continue.");
        ConsoleMethod.ReadKey();
    }

    /// <summary>
    /// Write products to file.
    /// </summary>
    /// <param name="newPath">New path to file.</param>
    private void HandlePathOutput(bool newPath)
    {
        if (newPath || _path.Length == 0)
        {
            HandlePathInput();
        }
        
        // Saving output stream.
        TextWriter defaultOutput = Console.Out;
        bool dataSaved = false;
        
        try
        {
            // Setting output stream.
            using var sw = new StreamWriter(_path);
            Console.SetOut(sw);
            JsonParser.WriteJson(_products);
            dataSaved = true;
        }
        catch (SecurityException ex)
        {
            ConsoleMethod.NicePrint(MessageHelper.Get("SetInSecurityExceptionError"));
            ConsoleMethod.NicePrint(ex.Message, Color.Error);
        }
        finally
        {
            // Setting default output.
            Console.SetOut(defaultOutput);
        }

        if (!dataSaved)
        {
            return;
        }
        
        ConsoleMethod.NicePrint("Data saved. Press any key to continue.", Color.Primary);
        ConsoleMethod.ReadKey();
    }
    
    /// <summary>
    /// Get products from file.
    /// </summary>
    private void HandleFileInput()
    {
        HandlePathInput();
        
        try
        {
            // Setting input stream.
            using var sr = new StreamReader(_path);
            Console.SetIn(sr);

            Products = JsonParser.ReadJson();
        }
        catch (FileNotFoundException)
        {
            ConsoleMethod.NicePrint(MessageHelper.Get("FileNotFoundExceptionError"));
        }
        catch (SecurityException ex)
        {
            ConsoleMethod.NicePrint(MessageHelper.Get("SetInSecurityExceptionError"));
            ConsoleMethod.NicePrint(ex.Message, Color.Error);
        }
        finally
        {
            // Setting standard input.
            Stream standardInput = Console.OpenStandardInput();
            Console.SetIn(new StreamReader(standardInput));
        }
    }

    private void HandleConsoleInput()
    {
        ConsoleMethod.NicePrint(MessageHelper.Get("ConsoleInput"), Color.Condition);
        Products = JsonParser.ReadJson();
    }

    private void UpdatePanelTitle()
    {
        _panelTitle = MessageHelper.Get("WorkPanelMessage", 
            "COUNT", Products.Count.ToString());
    }

    private void HandlePathInput()
    {
        ConsoleMethod.NicePrint(MessageHelper.Get("FilePathInput"), Color.Condition);
        _path = ConsoleMethod.ReadLine();
    }
}