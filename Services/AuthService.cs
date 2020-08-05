using System;
using System.Collections.Generic;
using Shop.Data;
using Shop.Domain;

namespace Shop.Services
{
    public interface IAuthService
    {
        List<User> Users { get; set; }
        User Login();
    }

    public class AuthService : IAuthService
    {
        private Database database = new Database();
        public List<User> Users { get; set; }

        public User Login()
        {
            Users = database.UserRepository.GetAll();
            while (true)
            {
                System.Console.WriteLine("Podaj login i haslo");
                string login = Console.ReadLine();
                string password = Console.ReadLine();
                foreach (User user in Users)
                {
                    if (user.Username == login && user.Password == password)
                    {
                        System.Console.WriteLine("Pomyslnie zalogowano\n");
                        return user;
                    }
                }
                System.Console.WriteLine("Nie udalo sie zalogowac\n");
            }

        }
    }
}