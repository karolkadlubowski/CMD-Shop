using System;
using SQLite;

namespace Shop.Domain
{
    [Table("Products")]
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }

        private decimal price;
        public decimal Price
        {
            get { return price; }
            set { price = value < 0 ? Math.Abs(value) : value; }
            // {
            //     if (value < 0)
            //         price=Math.Abs(value);
            //     else
            //         price=value;
            // }
        }

        public DateTime Date { get; set; } = DateTime.Now;
        public int UserId { get; set; }
    }
}