using Microsoft.VisualBasic;
using SauceDemoPlaywrightCSharp.Models;
using SauceDemoPlaywrightCSharp.Pages;

namespace SauceDemoPlaywrightCSharp.Tests;

[TestClass]
public class CartTest : TestBase
{
    static Account Account = Account.GetData();
    static List<Product> Products = Product.GetData();
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
    }

    [TestMethod]
    public async Task CartShouldDisplayAddedProductsAsExpected()
    {
        var header = new Header(Page);
        await header.GoToCart();
        var cartPage = new CartPage(Page);
        await Expect(header.GetPageTitleLocator()).ToHaveTextAsync(cartPage.Title);

        // Sauce Labs Bike Light
        await Expect(cartPage.GetProductDescriptionByName(ProductA.Name)).ToHaveTextAsync(ProductA.Description);
        await Expect(cartPage.GetProductPriceByName(ProductA.Name)).ToHaveTextAsync($"${ProductA.Price}");
        await Expect(cartPage.GetProductQuantityByName(ProductA.Name)).ToHaveTextAsync("1");
        await Expect(cartPage.GetProductButtonByName(ProductA.Name)).ToHaveTextAsync("Remove");
        
        // Sauce Labs Bolt T-Shirt
        await Expect(cartPage.GetProductDescriptionByName(ProductB.Name)).ToHaveTextAsync(ProductB.Description);
        await Expect(cartPage.GetProductPriceByName(ProductB.Name)).ToHaveTextAsync($"${ProductB.Price}");
        await Expect(cartPage.GetProductQuantityByName(ProductB.Name)).ToHaveTextAsync("1");
        await Expect(cartPage.GetProductButtonByName(ProductB.Name)).ToHaveTextAsync("Remove");
    }

    [TestMethod]
    public async Task CartShouldRemoveProducts()
    {
        var header = new Header(Page);
        await header.GoToCart();
        var cartPage = new CartPage(Page);
        await Expect(header.GetPageTitleLocator()).ToHaveTextAsync(cartPage.Title);

        await cartPage.RemoveProductByName(ProductA.Name);
        await Expect(cartPage.GetProductItemLocatorByName(ProductA.Name)).ToBeHiddenAsync();
        
        await Expect(cartPage.GetProductDescriptionByName(ProductB.Name)).ToHaveTextAsync(ProductB.Description);
        await Expect(cartPage.GetProductPriceByName(ProductB.Name)).ToHaveTextAsync($"${ProductB.Price}");
        await Expect(cartPage.GetProductQuantityByName(ProductB.Name)).ToHaveTextAsync("1");
        await Expect(cartPage.GetProductButtonByName(ProductB.Name)).ToHaveTextAsync("Remove");

        await Expect(header.GetCartButtonCounterLocator()).ToHaveTextAsync("1");
    }

    [TestMethod]
    public async Task CartShouldContinueShopping()
    {
        var header = new Header(Page);
        await header.GoToCart();
        var cartPage = new CartPage(Page);
        await Expect(header.GetPageTitleLocator()).ToHaveTextAsync(cartPage.Title);

        await cartPage.ContinueShopping();
        var productListPage = new ProductListPage(Page);
        await Expect(header.GetPageTitleLocator()).ToHaveTextAsync(productListPage.Title);
    }

    [TestMethod]
    public async Task CartShouldNavigateToProduct()
    {
        var header = new Header(Page);
        await header.GoToCart();
        var cartPage = new CartPage(Page);
        await Expect(header.GetPageTitleLocator()).ToHaveTextAsync(cartPage.Title);

        await cartPage.GoToProduct(ProductA.Name);
        var productDetailsPage = new ProductDetailsPage(Page);
        await Expect(header.GetBackToProductLocator()).ToBeVisibleAsync();
        await Expect(productDetailsPage.GetProductNameLocator()).ToHaveTextAsync(ProductA.Name);
        await Expect(productDetailsPage.GetRemoveProductLocator()).ToHaveTextAsync("Remove");
    }

    [TestMethod]
    public async Task CartShouldCheckout()
    {
        var header = new Header(Page);
        await header.GoToCart();
        var cartPage = new CartPage(Page);
        await Expect(header.GetPageTitleLocator()).ToHaveTextAsync(cartPage.Title);
        
        await cartPage.Checkout();
        var checkoutStepOnePage = new CheckoutStepOnePage(Page);
        await Expect(header.GetPageTitleLocator()).ToHaveTextAsync(checkoutStepOnePage.Title);
    }
}