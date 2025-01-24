using Microsoft.Playwright;

namespace SauceDemoPlaywrightCSharp.Pages;

class CheckoutStepTwoPage(IPage page) : CartPage(page)
{
    public new string Title = "Checkout: Overview";

    public async Task Cancel()
    {
        await page.GetByText("Cancel", new PageGetByTextOptions { Exact = true }).ClickAsync();
    }

    public async Task Finish() 
    {
        await page.GetByText("Finish", new PageGetByTextOptions { Exact = true }).ClickAsync();
    }
}