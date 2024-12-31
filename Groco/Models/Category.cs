using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Groco.Models
{
    public class Category
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public int Discount { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        public List<Product>? Products { get; set; }
    }
}
