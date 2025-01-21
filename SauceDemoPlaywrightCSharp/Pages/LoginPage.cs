using Microsoft.Playwright;

namespace SauceDemoPlaywrightCSharp.Pages;

class LoginPage(IPage page) : BasePage(page)
{
    private string UserNameSelector = "[data-test=\"username\"]";
    private string PasswordSelector = "[data-test=\"password\"]";
    private string LoginButtonSelector = "[data-test=\"login-button\"]";
    private string ErrorContainerSelector = "[data-test=\"error\"]"; 

    public ILocator GetErrorContainerLocator() 
    {
        return page.Locator(ErrorContainerSelector);
    }

    public ILocator GetLoginButtonLocator() {
        return page.Locator(LoginButtonSelector);
    }

    public async Task Login(string userName, string password) 
    {
        await page.FillAsync(UserNameSelector, userName);
        await page.FillAsync(PasswordSelector, password);
        await page.ClickAsync(LoginButtonSelector);
    }

    public async Task FillUserName(string userName) 
    {
        await page.FillAsync(UserNameSelector, userName);
    }

    public async Task FillPassword(string password) 
    {
        await page.FillAsync(PasswordSelector, password);
    }

    public async Task ClickOnLoginButton()
    {
        await page.ClickAsync(LoginButtonSelector);
    }   
}