using SQLite;

namespace Shop.Domain
{
    [Table("Purchases")]
    public class Purchase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProductId{get;set;}

        public string ProductName { get; set; }
    }
}