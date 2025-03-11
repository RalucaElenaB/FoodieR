using FoodieR.Models.DbObject;//acest namespace conține definiția clasei Product, care este folosită în acest model.

namespace FoodieR.Models;


//clasă model care va fi folosita pentru a stoca articolele stocate în coșul de cumpărături al utilizatorului
public class ShoppingCartItem
{
    public int ShoppingCartItemId { get; set; }//produsul pe care il adaug in cos- cheia primara a tabelei ShoppingCartItems 

    public Product Product { get; set; } = default!;//Fiecare element din coș este asociat cu un produs(ce are nume, pret, descriere); = default!; → se folosește pentru a evita avertismentele legate de null.

    public int Amount { get; set; }// cantitatea- cate bucati din X produs sunt in cos

    public string? ShoppingCartId { get; set; }//ID-ul coșului de cumpărături: dacă un utilizator nu este logat, coșul poate fi identificat printr-un ID stocat în sesiune; Dacă utilizatorul este logat, acest ID poate fi legat de contul său din baza de date.
}
