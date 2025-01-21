using Microsoft.Playwright;

namespace SauceDemoPlaywrightCSharp.Pages;

class BasePage(IPage page)
{
    protected IPage _page = page;

    public async Task NavigateToMainPage(){
        await _page.GotoAsync("https://www.saucedemo.com/");
    }
}