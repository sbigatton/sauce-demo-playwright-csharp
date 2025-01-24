using Microsoft.Playwright;

namespace SauceDemoPlaywrightCSharp.Pages;

class CartPage(IPage page) : ProductListPage(page)
{
    public new string Title = "Your Cart";
    protected string QuantitySelector = "[data-test=\"item-quantity\"]";

    public ILocator GetProductQuantityByName(string name)
    {
        return GetProductItemLocatorByName(name).Locator(QuantitySelector);
    }

    public async Task Checkout() 
    {
        await page.GetByText("Checkout", new PageGetByTextOptions { Exact = true }).ClickAsync();
    }

    public async Task ContinueShopping()
    {
        await page.GetByText("Continue Shopping", new PageGetByTextOptions { Exact = true }).ClickAsync();        
    }
}