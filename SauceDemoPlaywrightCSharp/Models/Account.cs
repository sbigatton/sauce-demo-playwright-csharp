using Newtonsoft.Json;

namespace SauceDemoPlaywrightCSharp.Models;

class Account
{
    public string Username { get; set; }
    public string Password { get; set; }

    public static Account FromJSON(string json)
    {
        return JsonConvert.DeserializeObject<Account>(json);
    }
}