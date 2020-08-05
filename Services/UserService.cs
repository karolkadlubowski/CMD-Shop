using System.Linq;
using System.Collections.Generic;
using System;
using Shop.Data;
using Shop.Domain;

namespace Shop.Services
{
    public interface IUserService
    {
        bool Displayer(User user);

        void ShowAllProducts();

        void AddProduct(int id);

        void GetSortedProducts(string text, decimal min, decimal max);

        void DeleteProduct();

        void PurchaseProduct(User user);

        void Motherlode(User user);

        void ShowPurchaseHistory(int id);

        User logout();


    }
    public class UserService : IUserService
    {
        private Database database = new Database();
        public bool Displayer(User user)
        {
            while (true)
            {

                System.Console.WriteLine("Co chcesz zrobic?\nA Pokaz wszystkie produkty\nB Pokaz posortowane produkty (min/max - optional)\nC Wyswietl produkt o id\nD Dodaj nowy produkt\nE Usun produkt\nF Kup produkt\nG Doladuj konto\nH Pokaz historie zakupow\nI Wyloguj");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "A":
                        ShowAllProducts();
                        break;
                    case "B":
                        System.Console.WriteLine("Podaj wskazowke do szukania");
                        string text = Console.ReadLine();
                        System.Console.WriteLine("Podaj minimalna cena");
                        string sval = Console.ReadLine();
                        decimal min=sval=="" ? 0 : decimal.Parse(sval);
                        System.Console.WriteLine("Podaj maxymalna cena");
                        sval=Console.ReadLine();
                        decimal max=sval=="" ? decimal.MaxValue : decimal.Parse(sval);
                        GetSortedProducts(text,min,max);
                        break;
                    case "C":
                        ShowCertainProduct();
                        break;
                    case "D":
                        AddProduct(user.Id);
                        break;
                    case "E":
                        DeleteProduct();
                        break;
                    case "F":
                        PurchaseProduct(user);
                        break;
                    case "G":
                        Motherlode(user);
                        break;
                    case "H":
                    ShowPurchaseHistory(user.Id);
                        break;
                    case "I":
                        user=logout();
                        return true;
                    default:
                        return false;
                }
            }
        }

        public void ShowAllProducts()
        {
            System.Console.WriteLine("Lista produktow:\n");
            List<Product> list = database.ProductRepository.GetAll();
            foreach (Product product in list)
            {
                System.Console.WriteLine($"{product.Id} {product.Name}");
            }
            System.Console.WriteLine("\n");
        }
        public void ShowCertainProduct()
        {
            System.Console.WriteLine("Podaj Id produktu");
            int id = int.Parse(Console.ReadLine());
            Product product = database.ProductRepository.Get(id);
            if (product == null)
                System.Console.WriteLine("Produktu o podanym ID nie ma w bazie\n");
            else
                System.Console.WriteLine($"{product.Id} {product.Name} {product.Price} {product.Date} {product.UserId}\n");
        }
        public void AddProduct(int id)
        {
            Product product = new Product();
            System.Console.WriteLine("Podaj nazwe produktu");
            product.Name = Console.ReadLine();
            System.Console.WriteLine("Podaj cene produktu");
            product.Price = decimal.Parse(Console.ReadLine());
            product.UserId = id;
            if (database.ProductRepository.Insert(product) == 1)
                System.Console.WriteLine("Pomyslnie dodano produkt\n");
            else
                System.Console.WriteLine("Nie udalo sie dodac produktu\n");
        }

        public void GetSortedProducts(string text, decimal min, decimal max)
        {
            List<Product> products = database.ProductRepository.GetWhere(product => (product.Name.Contains(text) && product.Price >= min && product.Price <= max));
            products = products.OrderBy(products => products.Price).ToList();
            foreach (Product product in products)
                System.Console.WriteLine($"{product.Name} {product.Price}");
        }
        public void DeleteProduct()
        {
            System.Console.WriteLine("Podaj Id produktu ktory chcesz usunac");
            int id = int.Parse(Console.ReadLine());
            Product product = database.ProductRepository.Get(id);
            if (product != null && database.ProductRepository.Delete(product) == 1)
                System.Console.WriteLine("Udalo sie pomyslnie usunac produkt");
            else
                System.Console.WriteLine("Nie udalo sie usunac produktu o podanym ID");

        }

        public void PurchaseProduct(User user)
        {
            System.Console.WriteLine("Podaj Id produktu ktory chcesz kupic");
            int id = int.Parse(Console.ReadLine());
            Product product = database.ProductRepository.Get(id);
            if (product == null)
            {
                System.Console.WriteLine("Nie ma produktu o podanym ID\n");
                return;
            }
            if (product.UserId == user.Id)
            {
                System.Console.WriteLine("Nie mozna kupic swojego produktu\n");
                return;
            }
            if (user.Money < product.Price)
            {
                System.Console.WriteLine("Uzytkownik ma za malo kasy\n");
                return;
            }
            if (database.ProductRepository.Delete(product) != 1)
            {
                System.Console.WriteLine("Nie udalo sie usunac produktu\n");
                return;
            }
            Purchase purchase = new Purchase();
            purchase.ProductId = product.Id;
            purchase.UserId = user.Id;
            purchase.ProductName=product.Name;
            if (database.PurchaseRepository.Insert(purchase) != 1)
            {
                System.Console.WriteLine("Nie udalo sie dodac zakupu\n");
                return;
            }
            User user2 = database.UserRepository.Get(product.UserId);
            if (user2 == null)
            {
                System.Console.WriteLine("Uzytkownik ktory wystawia produkt nie istnieje\n");
                return;
            }
            user2.Money += product.Price;
            user.Money -= product.Price;
            user.PurchasesCount++;
            if (database.UserRepository.Update(user) == 1 && database.UserRepository.Update(user2) == 1)
                System.Console.WriteLine("Pomyslnie dokonano transakcji\n");
            else
                System.Console.WriteLine("Nie udalo sie zaktualizowac salda srodkow\n");
        }

        public void Motherlode(User user)
        {
            Random rng = new Random();
            user.Money = rng.Next(50001);
            if (database.UserRepository.Update(user) == 1)
                System.Console.WriteLine("Pomyslnie dodano kase\n");
            else
                System.Console.WriteLine("Nie udalo sie dodac kasy\n");
        }

        public void ShowPurchaseHistory(int id)
        {
            var purchases=database.PurchaseRepository.GetWhere(purchase=>purchase.UserId==id);
            foreach(Purchase purchase in purchases)
                System.Console.WriteLine(purchase.ProductName);
        }
        
        public User logout()
        {
            return null;
        }
    }
}