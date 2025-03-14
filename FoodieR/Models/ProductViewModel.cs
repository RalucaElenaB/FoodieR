using FoodieR.Models.DbObject;
using System.ComponentModel.DataAnnotations;

namespace FoodieR.Models;

public class ProductViewModel
{
    public int ProductId { get; set; }
    public string Name { get; set; }

    [DisplayFormat(DataFormatString = "{0:N2} €")]
    public decimal Price { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; }

    public IEnumerable<ReviewViewModel> Reviews { get; set; }

    public string Description { get; set; }

}
