using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodieR.Models.DbObject;

public class OrderLine
{
    [Key]
    public int Id { get; set; }

    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    [ForeignKey("OrderId")]
    public Order Order { get; set; }

    [ForeignKey("ProductId")]
    public Product Product { get; set; }
}
