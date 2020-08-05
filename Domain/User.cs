using SQLite;

namespace Shop.Domain
{

    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public int PurchasesCount { get; set; }
        public decimal Money {get; set;}

        public User() { }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}