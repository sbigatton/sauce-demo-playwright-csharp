using Newtonsoft.Json;

namespace SauceDemoPlaywrightCSharp.Models;

class Product
{
    string Name { get; set; }
    string Description { get; set; }
    string Image { get; set; }
    double Price { get; set; }

    public IList<Product> FromJSON(string json)
    {
        return JsonConvert.DeserializeObject<List<Product>>(json);
    }
}