using Newtonsoft.Json;

namespace SauceDemoPlaywrightCSharp.Models;

class Account
{
    public required string Username { get; set; }
    public required string Password { get; set; }

    public static Account GetData()
    {
        var fileContent = File.ReadAllText("./Data/Account.json");
        return JsonConvert.DeserializeObject<Account>(fileContent);
    }
}