namespace SiparisYonetimi.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Mail { get; set; }
    public string Password { get; set; }
    public string Rol { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}