using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodieR.Models.DbObject;

//un obiect din baza de date care stochează informațiile despre liniile unei comenzi: definește un rând dintr-o comandă, adică un produs specific comandat într-o anumită cantitate. O comandă (Order) poate avea mai multe rânduri, fiecare corespunzând unui produs (Product).
public class OrderLine
{
    [Key]
    public int Id { get; set; }//Cheia primară a tabelului OrderLine. Identifică unic fiecare rând dintr-o comandă.

    public int OrderId { get; set; }//Cheia străină către comanda căreia îi aparține această linie.
    public int ProductId { get; set; }//Cheia străină către produsul comandat.
    public int Quantity { get; set; }//Numărul de unități comandate din acest produs.
    public decimal Amount { get; set; }//Totalul costului pentru acest rând (Quantity * Price).
    public decimal Price { get; set; }// Prețul unitar al produsului la momentul comenzii.

    [ForeignKey("OrderId")]
    public Order Order { get; set; }//Relația cu obiectul Order (pentru a obține toate datele comenzii).

    [ForeignKey("ProductId")]
    public Product Product { get; set; }//Relația cu obiectul Product(pentru a obține detalii despre produs).
}
