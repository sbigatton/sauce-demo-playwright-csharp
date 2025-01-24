using SauceDemoPlaywrightCSharp.Models;
using SauceDemoPlaywrightCSharp.Pages;
using Header = SauceDemoPlaywrightCSharp.Pages.Header;

namespace SauceDemoPlaywrightCSharp.Tests;

[TestClass]
public class LoginTest : TestBase
{
    static Account Account = Account.GetData();

    [TestInitialize]
    public async Task Setup()
    {
        var basePage = new BasePage(Page);
        await basePage.NavigateToMainPage();
    }

    [TestMethod]
    public async Task LoginRedirectsUserToProducListPage()
    {        
        var loginPage = new LoginPage(Page);
        await loginPage.Login(Account.Username, Account.Password);
        var header = new Header(Page);
        var productListPage = new ProductListPage(Page);
        await Expect(header.GetPageTitleLocator()).ToHaveTextAsync(productListPage.Title);
        await Expect(header.GetCartButtonLocator()).ToBeVisibleAsync();     
    } 

    [TestMethod]
    public async Task LoginShouldDisplayInvalidCredentialError()
    {
        var loginPage = new LoginPage(Page);
        await loginPage.ClickOnLoginButton();
        await Expect(loginPage.GetErrorContainerLocator()).ToHaveTextAsync("Epic sadface: Username is required");
        await loginPage.FillUserName(Account.Username);
        await loginPage.ClickOnLoginButton();
        await Expect(loginPage.GetErrorContainerLocator()).ToHaveTextAsync("Epic sadface: Password is required");
        await loginPage.FillPassword("invalidPassword");
        await loginPage.ClickOnLoginButton();
        await Expect(loginPage.GetErrorContainerLocator()).ToHaveTextAsync("Epic sadface: Username and password do not match any user in this service");
    }

    [TestMethod]
    public async Task LoginShouldDisplayAnErrorNavigatingWithUserNotLoggedIn()
    {
        await Page.GotoAsync("https://www.saucedemo.com/inventory.html");        
        var loginPage = new LoginPage(Page);        
        await Expect(loginPage.GetErrorContainerLocator()).ToHaveTextAsync("Epic sadface: You can only access '/inventory.html' when you are logged in.");
    }

    [TestMethod]
    public async Task LogoutShouldRedirectToLoginPage()
    {
        var loginPage = new LoginPage(Page);
        await loginPage.Login(Account.Username, Account.Password);
        var header = new Header(Page);
        await header.OpenMenu();
        await header.Logout();
        await Expect(loginPage.GetLoginButtonLocator()).ToBeVisibleAsync();
    }
}