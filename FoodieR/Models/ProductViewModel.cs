using FoodieR.Models.DbObject;

namespace FoodieR.Models;

public class ProductViewModel
{
    public int ProductId { get; set; }
    public string Name { get; set; }

    public decimal Price { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; }

    public IEnumerable<ReviewViewModel> Reviews { get; set; }

   
}
