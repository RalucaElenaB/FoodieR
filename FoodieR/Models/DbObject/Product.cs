using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodieR.Models.DbObject;

public class Product
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }

    public decimal Price { get; set; }

    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
}
