using Microsoft.Playwright;

namespace SauceDemoPlaywrightCSharp.Pages;

class CheckoutCompletePage(IPage page) : BasePage(page)
{
    private string title = "Checkout: Complete!";
    private string successImageSelector = ".pony_express";
    private string thankYouMsgSelector = ".complete-header";
    private string orderMsgSelector = ".complete-text";

    public ILocator GetSuccessImage() {
        return page.Locator(successImageSelector);
    }

    public ILocator GetThankYouMsg() {
        return page.Locator(thankYouMsgSelector);
    }

    public ILocator GetOrderMsg() {
        return page.Locator(orderMsgSelector);
    }

    public async Task BackHome() {
        await page.GetByText("Finish", new PageGetByTextOptions { Exact = true }).ClickAsync();
    }
}