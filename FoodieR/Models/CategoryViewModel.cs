using FoodieR.Models.DbObject;

namespace FoodieR.Models;

public class CategoryViewModel
{
    public IEnumerable<Category> Categories { get; set; }

    public  bool IsAdmin { get; set; }
}
