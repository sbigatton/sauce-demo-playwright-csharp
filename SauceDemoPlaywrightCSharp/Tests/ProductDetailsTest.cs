using SauceDemoPlaywrightCSharp.Models;
using SauceDemoPlaywrightCSharp.Pages;

namespace SauceDemoPlaywrightCSharp.Tests;

[TestClass]
public class ProductDetailsTest : TestBase
{
    static Account Account = Account.GetData();
    static List<Product> Products = Product.GetData();
    static Product Product = Products.First(x => x.Name == "Sauce Labs Bike Light");

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
        var productDetailsPage = new ProductDetailsPage(Page);          
        await productListPage.GoToProduct(Product.Name);
        await Expect(productDetailsPage.GetProductNameLocator()).ToHaveTextAsync(Product.Name);
    }

    [TestMethod]
    public async Task ShouldDisplayAProductDetailsAsExpected()
    {        
        var productDetailsPage = new ProductDetailsPage(Page);
        await Expect(productDetailsPage.GetProductNameLocator()).ToHaveTextAsync(Product.Name);
        await Expect(productDetailsPage.GetProductDescriptionLocator()).ToHaveTextAsync(Product.Description);
        await Expect(productDetailsPage.GetProductPriceLocator()).ToHaveTextAsync($"${Product.Price}");
        await Expect(productDetailsPage.GetProductImageLocator()).ToHaveAttributeAsync("src", Product.Image);
        await Expect(productDetailsPage.GetAddProductToCartLocator()).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task ProductListShouldAddAndRemoveItemsToCartAndDisplayCorrectCount() 
    {        
        var header = new Header(Page);
        var productDetailsPage = new ProductDetailsPage(Page);
        await productDetailsPage.AddProductToCart();
        await Expect(header.GetCartButtonCounterLocator()).ToHaveTextAsync("1");
        await productDetailsPage.RemoveProduct();
        await Expect(header.GetCartButtonCounterLocator()).ToBeHiddenAsync();
        await Expect(productDetailsPage.GetAddProductToCartLocator()).ToBeVisibleAsync();
    }
}