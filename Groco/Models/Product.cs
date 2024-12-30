using System.ComponentModel.DataAnnotations;

namespace Groco.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Rating {  get; set; }
        public string ImageUrl {  get; set; }
    }
}
