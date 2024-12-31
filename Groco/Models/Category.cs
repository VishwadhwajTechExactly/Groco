namespace Groco.Models
{
    public class Category
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public int Discount { get; set; }
        public string ImageUrl { get; set; }
        public List<Product>? Products { get; set; }
    }
}
