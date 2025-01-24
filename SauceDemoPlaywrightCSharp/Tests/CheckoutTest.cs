using SauceDemoPlaywrightCSharp.Models;
using SauceDemoPlaywrightCSharp.Pages;

namespace SauceDemoPlaywrightCSharp.Tests;

[TestClass]
public class CheckoutTest : TestBase
{
    static string AccountData = File.ReadAllText(@$"{Directory.GetCurrentDirectory()}\Data\account.json");
    static string ProductsData = File.ReadAllText(@$"{Directory.GetCurrentDirectory()}\Data\products.json");
    static Account Account = Account.FromJSON(AccountData);
    static List<Product> Products = Product.FromJSON(ProductsData);
    static Product ProductA = Products.First(x => x.Name == "Sauce Labs Bike Light");
    static Product ProductB = Products.First(x => x.Name == "Sauce Labs Bolt T-Shirt");

    [TestInitialize]
    public async Task Setup()
    {
        var basePage = new BasePage(Page);
        await basePage.NavigateToMainPage();
        var loginPage = new LoginPage(Page);
        await loginPage.Login(Account.Username, Account.Password);
        var header = new Header(Page);
        var productListPage = new ProductListPage(Page);
        await Expect(header.GetPageTitleLocator()).ToHaveTextAsync(productListPage.Title);
        await Expect(header.GetCartButtonLocator()).ToBeVisibleAsync();      
        await productListPage.AddProductToCartByName(ProductA.Name);
        await productListPage.AddProductToCartByName(ProductB.Name);
        await Expect(header.GetCartButtonCounterLocator()).ToHaveTextAsync("2");
        await header.GoToCart();
        var cartPage = new CartPage(Page);
        await cartPage.Checkout();
        var checkoutStepOnePage = new CheckoutStepOnePage(Page);
        await Expect(header.GetPageTitleLocator()).ToHaveTextAsync(checkoutStepOnePage.Title);
    }

    [TestMethod]
    public async Task CheckoutShouldDisplayInformationFieldValidationMessages() 
    {
        var checkoutStepOnePage = new CheckoutStepOnePage(Page);
        await checkoutStepOnePage.Continue();
        await Expect(checkoutStepOnePage.GetErrorContainerLocator()).ToHaveTextAsync("Error: First Name is required");
        await checkoutStepOnePage.FillFirstName("Jason");
        await checkoutStepOnePage.Continue();
        await Expect(checkoutStepOnePage.GetErrorContainerLocator()).ToHaveTextAsync("Error: Last Name is required");
        await checkoutStepOnePage.FillLastName("Born");
        await checkoutStepOnePage.Continue();
        await Expect(checkoutStepOnePage.GetErrorContainerLocator()).ToHaveTextAsync("Error: Postal Code is required");
    }

    [TestMethod]
    public async Task CheckoutShouldCancelAndNavigateBackToCartPage() 
    {
        var checkoutStepOnePage = new CheckoutStepOnePage(Page);
        await checkoutStepOnePage.Cancel();
        var header = new Header(Page);
        var cartPage = new CartPage(Page);
        await Expect(header.GetPageTitleLocator()).ToHaveTextAsync(cartPage.Title);
        await Expect(cartPage.GetProductItemLocatorByName(ProductA.Name)).ToBeVisibleAsync();
        await Expect(cartPage.GetProductItemLocatorByName(ProductB.Name)).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task CheckoutShouldContinueToFinishCheckout() 
    {
        var checkoutStepOnePage = new CheckoutStepOnePage(Page);
        await checkoutStepOnePage.FillFirstName("Jason");
        await checkoutStepOnePage.FillLastName("Born");
        await checkoutStepOnePage.FillZipCode("19019");
        await checkoutStepOnePage.Continue();
        var checkoutStepTwoPage = new CheckoutStepTwoPage(Page);
        var header = new Header(Page);
        await Expect(header.GetPageTitleLocator()).ToHaveTextAsync(checkoutStepTwoPage.Title);
    }


    [TestMethod]
    public async Task CheckoutShouldCancelAndNavigateBackToProductsPage() 
    {
        var checkoutStepOnePage = new CheckoutStepOnePage(Page);
        await checkoutStepOnePage.FillInformationAndContinue("Jason", "Born", "19019");

        var checkoutStepTwoPage = new CheckoutStepTwoPage(Page);
        await checkoutStepTwoPage.Cancel();
        var header = new Header(Page);
        var productListPage = new ProductListPage(Page);
        await Expect(header.GetPageTitleLocator()).ToHaveTextAsync(productListPage.Title);      
        await Expect(header.GetCartButtonCounterLocator()).ToHaveTextAsync("2");
        await Expect(productListPage.GetProductButtonByName(ProductA.Name)).ToHaveTextAsync("Remove");
        await Expect(productListPage.GetProductButtonByName(ProductB.Name)).ToHaveTextAsync("Remove");
    }

    [TestMethod]
    public async Task CheckoutShouldDisplayProductDetailsAsExpected() 
    {
        var checkoutStepOnePage = new CheckoutStepOnePage(Page);
        await checkoutStepOnePage.FillInformationAndContinue("Jason", "Born", "19019");

        var checkoutStepTwoPage = new CheckoutStepTwoPage(Page);

        // Sauce Labs Bike Light
        await Expect(checkoutStepTwoPage.GetProductDescriptionByName(ProductA.Name)).ToHaveTextAsync(ProductA.Description);
        await Expect(checkoutStepTwoPage.GetProductPriceByName(ProductA.Name)).ToHaveTextAsync($"${ProductA.Price}");
        await Expect(checkoutStepTwoPage.GetProductQuantityByName(ProductA.Name)).ToHaveTextAsync("1");
        
        // Sauce Labs Bolt T-Shirt
        await Expect(checkoutStepTwoPage.GetProductDescriptionByName(ProductB.Name)).ToHaveTextAsync(ProductB.Description);
        await Expect(checkoutStepTwoPage.GetProductPriceByName(ProductB.Name)).ToHaveTextAsync($"${ProductB.Price}");
        await Expect(checkoutStepTwoPage.GetProductQuantityByName(ProductB.Name)).ToHaveTextAsync("1");
    }

    [TestMethod]
    public async Task CheckoutShouldCompleteTheOrder() 
    {
        var checkoutStepOnePage = new CheckoutStepOnePage(Page);
        await checkoutStepOnePage.FillInformationAndContinue("Jason", "Born", "19019");

        var checkoutStepTwoPage = new CheckoutStepTwoPage(Page);
        await checkoutStepTwoPage.Finish();

        var header = new Header(Page);
        var checkoutCompletePage = new CheckoutCompletePage(Page);
        await Expect(header.GetPageTitleLocator()).ToHaveTextAsync(checkoutCompletePage.Title);
        await Expect(checkoutCompletePage.GetThankYouMsg()).ToBeVisibleAsync();
        await Expect(checkoutCompletePage.GetSuccessImage()).ToBeVisibleAsync();
        await Expect(checkoutCompletePage.GetOrderMsg()).ToHaveTextAsync("Your order has been dispatched, and will arrive just as fast as the pony can get there!");
    }
}