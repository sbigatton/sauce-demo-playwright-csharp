using Microsoft.Playwright;

namespace SauceDemoPlaywrightCSharp.Pages;

class ProductListPage(IPage page) : ProductDetailsPage(page)
{
    public string Title = "Products";
    private string ProductsImageSelector = "img.inventory_item_img"; 

    public ILocator GetProductItemLocatorByName(string name) 
    {
        return page.Locator(ProductItemSelector, new PageLocatorOptions { Has = page.GetByText(name) });        
    }

    public ILocator GetProductDescriptionByName(string name) {
         return GetProductItemLocatorByName(name).Locator(ProductDescriptionSelector);
    }

    public ILocator GetProductPriceByName(string name) {
        return GetProductItemLocatorByName(name).Locator(ProductPriceSelector);
    }

    public ILocator GetProductImageByName(string name) {
        return GetProductItemLocatorByName(name).Locator(ProductsImageSelector);
    }

    public ILocator GetProductButtonByName(string name) {
        return GetProductItemLocatorByName(name).Locator("button");
    }

    public async Task AddProductToCartByName(string name) 
    {
        await GetProductItemLocatorByName(name).GetByText("Add to cart", new LocatorGetByTextOptions { Exact = true }).ClickAsync();
    }

    public async Task RemoveProductByName(string name) {
        await GetProductItemLocatorByName(name).GetByText("Remove", new LocatorGetByTextOptions { Exact = true }).ClickAsync();
    }

    public async Task GoToProduct(string name) {
        await GetProductItemLocatorByName(name).Locator(ProductNameSelector, new LocatorLocatorOptions { HasText = name }).ClickAsync();
    }
}