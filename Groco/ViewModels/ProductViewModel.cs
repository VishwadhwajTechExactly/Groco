using Groco.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Groco.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
