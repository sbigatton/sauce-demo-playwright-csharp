using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace SauceDemoPlaywrightCSharp.Tests;

[TestClass]
public class TestBase : PageTest
{
    [TestCleanup]
    public async Task TestCleanup()
    {
        // Check if the test failed
        if (TestContext.CurrentTestOutcome == UnitTestOutcome.Failed)
        {
            string fileName = $"{TestContext.TestName}-{DateTime.Now:yyyyMMddHHmmss}.png";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Test Reports", fileName);

            // Take screenshot
            await Page.ScreenshotAsync(new PageScreenshotOptions { Path = filePath, FullPage = true });
            Console.WriteLine($"Screenshot taken: {filePath}");
        }
    }
}