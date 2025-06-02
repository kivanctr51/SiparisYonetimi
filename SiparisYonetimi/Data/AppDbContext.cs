using Microsoft.EntityFrameworkCore;
using SiparisYonetimi.Models;

namespace SiparisYonetimi;

public class AppDbContext: DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer("Server=localhost;Database=SiparisDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True");
    }

}

