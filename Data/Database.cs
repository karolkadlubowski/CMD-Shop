using Shop.Domain;
using SQLite;

namespace Shop.Data
{
    public interface IDatabase
    {
        IRepository<User> UserRepository { get; }
        IRepository<Product> ProductRepository { get; }
        IRepository<Purchase> PurchaseRepository { get; }

        void CreateTable<T>() where T : class, new();
    }

    public class Database : IDatabase
    {
        private readonly SQLiteConnection connection;

        private const string DatabaseLocation = @"D:\Studia\Projekty\Shop\Data\database.db";

        private IRepository<User> userRepository;
        public IRepository<User> UserRepository
        {
            get => userRepository ?? new Repository<User>(connection);
        }

        private IRepository<Product> productRepository;
        public IRepository<Product> ProductRepository
        {
            get => productRepository ?? new Repository<Product>(connection);
        }

        private IRepository<Purchase> purchaseRepository;
        public IRepository<Purchase> PurchaseRepository
        {
            get => purchaseRepository ?? new Repository<Purchase>(connection);
        }

        public Database()
        {
            this.connection = new SQLiteConnection(DatabaseLocation);
        }

        public void CreateTable<T>() where T : class, new()
        {
            connection.CreateTable<T>();
        }
    }
}