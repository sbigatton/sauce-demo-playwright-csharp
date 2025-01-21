using Microsoft.Playwright;

namespace SauceDemoPlaywrightCSharp.Pages;

class CheckoutStepOnePage(IPage page) : ProductDetailsPage(page)
{
    private string Title = "Checkout: Your Information";
    private string FirstNameSelector = "[data-test=\"firstName\"]";
    private string LastNameSelector = "[data-test=\"lastName\"]";
    private string ZipCodeSelector = "[data-test=\"postalCode\"]";
    private string ErrorContainerSelector = ".error-message-container";

    public ILocator GetErrorContainerLocator()
    {
        return page.Locator(ErrorContainerSelector);
    }

    public async Task FillFirstName(string firstName) 
    {
        await page.FillAsync(FirstNameSelector, firstName);
    }

    public async Task FillLastName(string lastName) 
    {
        await page.FillAsync(LastNameSelector, lastName);
    }

    public async Task FillZipCode(string code) 
    {
        await page.FillAsync(ZipCodeSelector, code);
    }

    public async Task Continue() 
    {
        await page.GetByText("Continue", new PageGetByTextOptions { Exact = true }).ClickAsync();
    }

    public async Task Cancel() 
    {
        await page.GetByText("Cancel", new PageGetByTextOptions { Exact = true }).ClickAsync();
    }
}