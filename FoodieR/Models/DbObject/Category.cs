using System.ComponentModel.DataAnnotations;

namespace FoodieR.Models.DbObject;

public class Category
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }

    public IEnumerable<Product> Products { get; set; }
}
