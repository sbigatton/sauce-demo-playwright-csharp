using Newtonsoft.Json;

namespace SauceDemoPlaywrightCSharp.Models;

class Product
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public double Price { get; set; }

    public static List<Product> FromJSON(string json)
    {
        return JsonConvert.DeserializeObject<List<Product>>(json);
    }
}