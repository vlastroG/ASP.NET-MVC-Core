using System.ComponentModel.DataAnnotations;

namespace RazorPagesGB.Models
{
    public class Product
    {
        private static int _lastId = 0;
        public int Id { get; private set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        public string Image { get; set; }


        public Product() : this("Default", String.Empty, 99, String.Empty) { }


        public Product(string name, string description, double price, string image)
        {
            Id = ++_lastId;
            Name = name;
            Description = description;
            Price = price;
            Image = @image;
        }
    }
}
