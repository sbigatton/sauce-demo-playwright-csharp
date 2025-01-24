using SauceDemoPlaywrightCSharp.Models;
using SauceDemoPlaywrightCSharp.Pages;

namespace SauceDemoPlaywrightCSharp.Tests;

[TestClass]
public class ProductListTest : TestBase
{
    static string AccountData = File.ReadAllText(@$"{Directory.GetCurrentDirectory()}\Data\account.json");
    static string ProductsData = File.ReadAllText(@$"{Directory.GetCurrentDirectory()}\Data\products.json");
    static Account Account = Account.FromJSON(AccountData);
    static List<Product> Products = Product.FromJSON(ProductsData);
    static string[] ProductNames = Products.Select(x => x.Name).ToArray();

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
    }

    [TestMethod]
    public async Task ProductListShouldDisplayExpectedListOfProducts()
    {        
        var productListPage = new ProductListPage(Page);
        await Expect(productListPage.GetProductNameLocator()).ToHaveTextAsync(ProductNames);        
    }

    [TestMethod]
    public async Task ProductListShouldDisplayAProductDetailAsExpected()
    {        
        var product = Products.First(product => product.Name == "Sauce Labs Bike Light");
        var productListPage = new ProductListPage(Page); 
        await Expect(productListPage.GetProductDescriptionByName(product.Name)).ToHaveTextAsync(product.Description);
        await Expect(productListPage.GetProductPriceByName(product.Name)).ToHaveTextAsync($"${product.Price}");
        await Expect(productListPage.GetProductImageByName(product.Name)).ToHaveAttributeAsync("src", product.Image);
        await Expect(productListPage.GetProductButtonByName(product.Name)).ToHaveTextAsync("Add to cart");
    } 

    [TestMethod]
    public async Task ProductListShouldAddAndRemoveItemsToCartAndDisplayCorrectCount()
    {
        var header = new Header(Page);
        var productListPage = new ProductListPage(Page);
        await productListPage.AddProductToCartByName(ProductNames[1]);
        await Expect(header.GetCartButtonCounterLocator()).ToHaveTextAsync("1");
        await Expect(productListPage.GetProductButtonByName(ProductNames[1])).ToHaveTextAsync("Remove");
        await productListPage.AddProductToCartByName(ProductNames[2]);
        await Expect(header.GetCartButtonCounterLocator()).ToHaveTextAsync("2");
        await Expect(productListPage.GetProductButtonByName(ProductNames[2])).ToHaveTextAsync("Remove");
        await productListPage.RemoveProductByName(ProductNames[1]);
        await Expect(header.GetCartButtonCounterLocator()).ToHaveTextAsync("1");
        await Expect(productListPage.GetProductButtonByName(ProductNames[1])).ToHaveTextAsync("Add to cart");
    }

    [TestMethod]
    public async Task ProductListShouldSortProductsProperly()
    {
        var header = new Header(Page);
        var productListPage = new ProductListPage(Page);

        await Expect(header.GetActiveFilterOptionLocator()).ToHaveTextAsync("Name (A to Z)");
        var productNamesAtoZ = ProductNames.Order();
        await Expect(productListPage.GetProductNameLocator()).ToHaveTextAsync(productNamesAtoZ);
        
        await header.SelectFilterOption("Name (Z to A)");
        var productNamesZtoA = ProductNames.OrderDescending();
        await Expect(productListPage.GetProductNameLocator()).ToHaveTextAsync(productNamesZtoA);

        await header.SelectFilterOption("Price (low to high)");
        var lowToHighProductNames = Products.OrderBy(p => p.Price).Select(x => x.Name);
        await Expect(productListPage.GetProductNameLocator()).ToHaveTextAsync(lowToHighProductNames);

        await header.SelectFilterOption("Price (high to low)");
        var highToLowProductNames = Products.OrderByDescending(p => p.Price).Select(x => x.Name);
        await Expect(productListPage.GetProductNameLocator()).ToHaveTextAsync(highToLowProductNames);
    }
}