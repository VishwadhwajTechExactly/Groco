using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Groco.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int ProductId {  get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public  Product Product { get; set; }
        public int Count {  get; set; }
        public string UserId {  get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public IdentityUser User { get; set; }

    }
}
