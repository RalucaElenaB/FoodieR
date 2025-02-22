using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodieR.Models.DbObject;

public class Order
{

    [Key]
    public int Id { get; set; }

    public DateTime CreateDate { get; set; }
    public decimal TotalAmount { get; set; }

    public IEnumerable<Product> Products { get; set; }
}
