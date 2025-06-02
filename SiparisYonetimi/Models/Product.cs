namespace SiparisYonetimi.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Detail { get; set; }
    
    public int Price { get; set; }
    public string Stock { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

}