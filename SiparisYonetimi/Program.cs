using System.Security.Cryptography;
using System.Text;
using SiparisYonetimi;
using SiparisYonetimi.Migrations;
using SiparisYonetimi.Models;
using Product = SiparisYonetimi.Models.Product;

namespace SiparisYonetimi;

class Program
{
    static void Main(string[] args)
    {

        var mainMenu = new ConsoleMenu("------- Siparis Sistemi ------", true);
     
        mainMenu.AddMenu("Kullanıcı Kayıt", KullaniciKayit);
        mainMenu.AddMenu("Kullanıcı Girişi", KullaniciGirisi);
        mainMenu.Show();

    }

    static void KullaniciKayit()
    {
        var db = new AppDbContext();


        Console.Clear();
        Console.WriteLine("----- KAYIT OL -----");
        var Name = Helper.Ask("İsiminiz ", true);
        var LastName = Helper.Ask("Soyisim ", true);
        var Email = Helper.Ask("E-posta ", true);
        var Pass = Helper.Ask("Şifre ", true);
        var Rol = Helper.Ask("Rol (admin/kullanıcı ", true).ToLower();

        var existingUser = db.Users.FirstOrDefault(u => u.Mail == Email);
        
        if (existingUser != null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Bu mail adresiyle daha önce kayıt olunmuş.");
            Thread.Sleep(2000);
            Console.ResetColor();
            return;
        }

        if (Rol == "admin" || Rol == "kullanıcı")
        {
            var newUser = new User()
            {
                Name = Name,
                LastName = LastName,
                Mail = Email,
                Password = Pass,
                Rol = Rol,
            };

            if (newUser == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("kayıt Başarısız");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Kayıt Başarılı Şekilde oluşturuldu");
                db.Add(newUser);
                db.SaveChanges();
                Thread.Sleep(2000);
            
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("lutfen admin veya kullanıcı oldugunuzu belirtin");
            Thread.Sleep(2000);
            Console.ResetColor();
            KullaniciKayit();
        }
    }

    static void KullaniciGirisi()
    {
        var db = new AppDbContext();
        Console.WriteLine("----- GİRİŞ YAP -----");
        var loginUserMail = Helper.Ask("Mail Adres", true);
        var loginUserPass = Helper.Ask("Şifre ", true);
        
        
        
        
       var user = db.Users.Where(k => k.Mail == loginUserMail && k.Password == loginUserPass).FirstOrDefault();

       if (user != null)
       {
           Console.WriteLine("Kullanıcı bulundu: " +" "+ user.Mail);
           
          

           if (user.Rol == "admin")
           {
              
               AdminPanel();
               Thread.Sleep(2000);
           }
           
           else if (user.Rol == "kullanıcı")
           {
              
              UserPanel(loginUserMail:user.Mail);
              Console.WriteLine(" kullanıcı Panel");
              Thread.Sleep(2000);
           }
           else
           {
               Console.ForegroundColor = ConsoleColor.Red;
               Console.WriteLine("başarısız");
               Thread.Sleep(2000);
               Console.ResetColor();
           }
       }
       else
       {
           Console.ForegroundColor = ConsoleColor.Red;
           Console.WriteLine("Mail veya Şifre Hatalı.");
           Console.ResetColor();
           Thread.Sleep(2000);
           Console.Clear();
           KullaniciGirisi();
       }

       
    }

    
    static void AdminPanel()
    {
        Console.WriteLine("Yönetim Paneline hoşgeldiniz.");
        Thread.Sleep(2000);
        
        var mainMenu = new ConsoleMenu("Yönetim Paneli", true);
        mainMenu.AddMenu("Tüm kullanıcıları Görüntüle", AllUsers );
        mainMenu.AddMenu("Tüm Stokklar", AllStocks );
        mainMenu.AddMenu("Stok Günceleme" ,stockUpdate );
        mainMenu.AddMenu("Yeni Ürün Ekle" , AddNewProduct );
        mainMenu.AddMenu("Rapor görüntüle" , () => Console.WriteLine("a") );
        
        
        mainMenu.Show();
        
    }
    
     public static void UserPanel(string loginUserMail)
    {
        Console.WriteLine("kullanıcı paneline hoşgeldiniz.");
        Thread.Sleep(2000);
        var mainMenu = new ConsoleMenu("Yönetim Paneli", true);
        mainMenu.AddMenu("Siparis ver", () => PlaceAnOrder(loginUserMail));
        mainMenu.AddMenu("Sipariş Durumu" , OrderStatus );
        
        mainMenu.Show();
    }

 


    // YöNETİM bolumu 
    static void AddNewProduct()
    {
        var db = new AppDbContext();
        Console.WriteLine("----- ÜRÜN GİRİŞİ -----");
        var productName = Helper.Ask("Ürün İsmi giriniz");
        var productDetails = Helper.Ask("Ürün detayları");
        var productStock = Helper.Ask("Stok Adedi");

        var newProduct = new Product()
        {
            Name = productName,
            Detail = productDetails,
            Stock = productStock
        };
        if (newProduct == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("kayıt başarısız");
            Thread.Sleep(2000);
            Console.ResetColor();
        }
        else
        {
            
            db.Add(newProduct);
            db.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("ürün kaydı Başarılı şekilde tamamlandı");
            Console.ResetColor();
            Thread.Sleep(2000);
            

        }

        
        
    }
    static void AllStocks()
    {
        var db = new AppDbContext();
        var stocks = db.Products.ToArray();

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("----- Stok Listesi -----");
        Console.ResetColor();

        foreach (var stock in stocks)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"ID: {stock.Id} - Ürün: {stock.Name} - Açıklama: {stock.Detail} - Stok: {stock.Stock}");
        }

        Console.ResetColor();
        Console.WriteLine("--------------------------");

        Console.WriteLine("Devam etmek için bir tuşa basın...");
        Console.ReadKey();
    }

    static void AllUsers()
    {
        var db = new AppDbContext();
        var users = db.Users.ToArray();
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("----- Stok Listesi -----");
        Console.ResetColor();

        foreach (var user in users)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"ID:  {user.Id} İsim:  {user.Name} Soyisim:  {user.LastName} Mail: {user.Mail}  password:  {user.Password} Rol: {user.Rol}  Kayıt Tarihi:  {user.CreatedAt}");
        }

        Console.ResetColor();
        Console.WriteLine("--------------------------");

        Console.WriteLine("Devam etmek için bir tuşa basın...");
        Console.ReadKey();
    }

    static void stockUpdate()
    {
        var db = new AppDbContext();
        
        var status = db.Products.Select(p => new {p.Id,p.Name, p.Stock}).ToList();
        Console.WriteLine("-------- STOCK GÜNCELLEME ---------");
        foreach (var statuss in status)
        {
            
            Console.WriteLine($"{statuss.Id} , {statuss.Name}, {statuss.Stock}");
        }
        
        Console.WriteLine("--------------------------");
     
        int stocksID = int.Parse(Helper.Ask("Stoğu güncelemek istediğiniz ürün ID'sini giriniz"));

        var dbCount = db.Products.Count();
        if (stocksID > dbCount)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("boyle bir urun yok");
            Console.ResetColor();
            Thread.Sleep(3000);
            return;
        }
        
        var stocks = db.Products.FirstOrDefault(f => f.Id == stocksID);
        if (stocksID == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ürün bulunamadı");
            Console.ResetColor();
            stockUpdate();
        }
        
        
        var newstocks = Helper.Ask($"Yeni stok miktarı giriniz: {stocks.Stock}");
        stocks.Stock = newstocks;
        db.SaveChanges();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Stock Başarılı Şekilde Güncellendi");
        Console.ResetColor();
        Thread.Sleep(3000);
            
       
       
        

    }
    // müşteri sipariş bolumu 
     private static void PlaceAnOrder(string loginUserMail)
    {
        var db = new AppDbContext();
        
        var status = db.Products.Select(p => new {p.Id,p.Name, p.Stock,p.Price}).ToList();
        Console.WriteLine("-------- MENU ---------");
        foreach (var statuss in status)
        {
            
            Console.WriteLine($"{statuss.Id} --- {statuss.Name} --- Fiyat : {statuss.Price}TL");
            Console.WriteLine("------------------------------------------------");
           
        }
        var order = int.Parse(Helper.Ask("Siparis vermek istediğniz yemeğin ID'sini giriniz"));;



        var productOrders = db.Products.FirstOrDefault(o => o.Id == order);

       
        var NewOrder = new Order()
        {
            User = loginUserMail,
            OrderName =  productOrders.Name,
            OrderPrice = productOrders.Price,
            
        };
        
        if (NewOrder != null)
        {
            db.Orders.Add(NewOrder);
            db.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Sipariş Başarılı Şekilde Oluşturuldu");
            Console.ResetColor();
            Thread.Sleep(3000);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("siparis olustuırulamadı");
            Console.ResetColor();
            Thread.Sleep(3000);
        }
        


    }

    static void OrderStatus()
    {
        var db = new AppDbContext();

        foreach (var orderr in db.Orders)
        {
            Console.WriteLine($"{orderr.Id} {orderr.OrderName} Fiyat : {orderr.OrderPrice} Sipariş Durumu : {orderr.OrderDate} Siparis Tarihi : {orderr.Datetime}");
            Console.WriteLine("--------------------------");
        }
       

        Console.WriteLine("Devam etmek için bir tuşa basın...");
        Console.ReadKey();

    }
}
    
   
