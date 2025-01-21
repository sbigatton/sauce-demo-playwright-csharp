using Microsoft.Playwright;

namespace SauceDemoPlaywrightCSharp.Pages;

class ProductDetailsPage(IPage page) : BasePage(page)
{
    private string Title = "Back to products";
    protected string ProductItemSelector = "[data-test=\"inventory-item\"]";
    protected string ProductNameSelector = "[data-test=\"inventory-item-name\"]";
    protected string ProductDescriptionSelector = "[data-test=\"inventory-item-desc\"]";
    protected string ProductPriceSelector = "[data-test=\"inventory-item-price\"]";
    private string ProductImageSelector = "img.inventory_details_img";

    public ILocator GetProductItemLocator() 
    {
        return page.Locator(ProductItemSelector);
    }

    public ILocator GetProductNameLocator()  
    {
        return page.Locator(ProductNameSelector);
    }

    public ILocator GetProductDescriptionLocator()  
    {
        return page.Locator(ProductDescriptionSelector);
    }

    public ILocator GetProductPriceLocator()  
    {
        return page.Locator(ProductPriceSelector);
    }

    public ILocator GetProductImageLocator()        
    {
        return page.Locator(ProductImageSelector);
    }

    public ILocator GetAddProductToCartLocator()        
    {
        return page.GetByText("Add to cart", new PageGetByTextOptions { Exact = true});
    }

    public ILocator GetRemoveProductLocator()        
    {
        return page.GetByText("Remove", new PageGetByTextOptions { Exact = true});    
    }

    public async Task AddProductToCart() 
    {        
        await GetAddProductToCartLocator().ClickAsync();
    }

    public async Task RemoveProduct() 
    {        
        await GetRemoveProductLocator().ClickAsync();
    }
}