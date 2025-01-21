using Microsoft.Playwright;

namespace SauceDemoPlaywrightCSharp.Pages;

class Header(IPage page) : BasePage(page)
{
    private string Title = "Swag Labs";
    private string MenuSelector = "button#react-burger-menu-btn";
    private string CartButtonSelector = "[data-test=\"shopping-cart-link\"]";
    private string CartButtonCounterSelector = "[data-test=\"shopping-cart-badge\"]";
    private string PageTitleSelector = "[data-test=\"title\"]";
    private string ActiveFilterOptionSelector = "[data-test=\"active-option\"]";
    private string SelectFilterSelector = "[data-test=\"product-sort-container\"]";
    private string BackToProductSelector = "#back-to-products";


    public ILocator GetCartButtonLocator() 
    {
        return page.Locator(CartButtonSelector);
    }

    public ILocator GetCartButtonCounterLocator()
     {
        return page.Locator(CartButtonCounterSelector);
    }

    public ILocator GetPageTitleLocator() 
    {
        return page.Locator(PageTitleSelector);
    }

    public ILocator GetActiveFilterOptionLocator() 
    {
        return page.Locator(ActiveFilterOptionSelector);
    }

    public ILocator GetBackToProductLocator() 
    {
        return page.Locator(BackToProductSelector);
    }
    public ILocator GetFilterOptionsLocator()
    {
        return GetSelectFilterOptionLocator().Locator("option");
    }

    public ILocator GetSelectFilterOptionLocator() 
    {
        return page.Locator(SelectFilterSelector);
    }

    public async Task SelectFilterOption(string option) 
    {
        await GetSelectFilterOptionLocator().SelectOptionAsync(option);
    }

    public async Task GoToCart() 
    {
        await GetCartButtonLocator().ClickAsync();
    }

    public async Task GoBackToProducts() 
    {
        await GetBackToProductLocator().ClickAsync();
    }

    public async Task OpenMenu()
    {
        await page.ClickAsync(MenuSelector);
    }

    public async Task Logout()
    {
        await page.GetByText("Logout").ClickAsync();
    }
}