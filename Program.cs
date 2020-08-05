using System.Collections.Generic;
using Shop.Data;
using Shop.Domain;
using Shop.Services;

namespace Shop
{
    class Program
    {
        private static User user;
        static void Main(string[] args)
        {
            //Init database
            var database = new Database();
            InitDatabase(database);

            //Code
            IAuthService authService=new AuthService();
            while(true)
            {
                user = authService.Login();
                IUserService userService = new UserService();
                if(!userService.Displayer(user))
                    return;
            }
        }

        #region initDatabase

        private static void InitDatabase(Database database)
        {
            database.CreateTable<User>();
            database.CreateTable<Product>();
            database.CreateTable<Purchase>();

            if (database.UserRepository.GetAll().Count == 0)
            {
                var users = new List<User>
                {
                    new User("tomasz", "password"),
                    new User("filip", "pyziakowski"),
                    new User("karol", "jubiler"),
                    new User("tomczak", "kubal40"),
                    new User("mikegor", "brumbrum")
                };

                users.ForEach(u => database.UserRepository.Insert(u));
            }
        }

        #endregion
    }
}
