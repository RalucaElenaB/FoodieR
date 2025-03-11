using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FoodieR.Models.DbObject;

//clasa reprezintă o comandă făcută de un client
public class Order
{

    [Key]
    public int Id { get; set; }//Cheia primară a comenzii, identifică unic fiecare comandă în baza de date.

    public DateTime CreateDate { get; set; }//Data și ora la care comanda a fost plasată.
    public decimal TotalAmount { get; set; }//Suma totală a comenzii (calculată prin însumarea Amount din OrderLine).
    public List<OrderLine> OrderLines { get; set; }//detaliile comenzii: O listă care conține toate produsele incluse în comandă (relație 1 la multe cu OrderLine); face legătura cu toate produsele din comandă.

    public IdentityUser Customer { get; set; }//Utilizatorul care a plasat comanda, reprezentat de clasa IdentityUser din ASP.NET Identity.
}