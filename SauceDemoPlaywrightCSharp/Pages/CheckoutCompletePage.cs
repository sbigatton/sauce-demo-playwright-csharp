using Microsoft.Playwright;

namespace SauceDemoPlaywrightCSharp.Pages;

class CheckoutCompletePage(IPage page) : BasePage(page)
{
    public string Title = "Checkout: Complete!";
    private string SuccessImageSelector = ".pony_express";
    private string ThankYouMsgSelector = ".complete-header";
    private string OrderMsgSelector = ".complete-text";

    public ILocator GetSuccessImage() {
        return page.Locator(SuccessImageSelector);
    }

    public ILocator GetThankYouMsg() {
        return page.Locator(ThankYouMsgSelector);
    }

    public ILocator GetOrderMsg() {
        return page.Locator(OrderMsgSelector);
    }

    public async Task BackHome() {
        await page.GetByText("Finish", new PageGetByTextOptions { Exact = true }).ClickAsync();
    }
}