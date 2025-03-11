using FoodieR.Data;//Importă FoodieR.Data → Conține ApplicationDbContext, adică conexiunea la baza de date.
using FoodieR.Models.DbObject;//Importă FoodieR.Models.DbObject → Conține definițiile modelelor (Product, ShoppingCartItem).
using Microsoft.EntityFrameworkCore;// Importă Microsoft.EntityFrameworkCore → Folosit pentru interogări asupra bazei de date cu Entity Framework Core.

namespace FoodieR.Models;//Definește locul unde se află această clasă în proiect.

//gestionarea(implementarea) cosului de cumparaturi; definesc câteva metode pentru a lucra cu un coș de cumpărături: adăugare, eliminare, golire și calculul totalului coșului.
public class ShoppingCart
{
   
    private readonly ApplicationDbContext _context;//_context: variabilă privată folosită pentru a interacționa cu baza de date.

    public string? ShoppingCartId { get; set; }//Identificator unic pentru coșul de cumpărături (stocat în sesiune);poate fi null (?) în cazul în care nu a fost inițializat.

    public List<ShoppingCartItem> ShoppingCartItems { get; set; } = default;// Lista produselor din coș; este inițializată implicit cu default (în implementare va fi o listă de ShoppingCartItem).

    private ShoppingCart(ApplicationDbContext context)//injectex DbContext in constructor care e private → nu poate fi apelat direct din alte clase;primește ApplicationDbContext → necesită baza de date pentru a funcționa.
    {
        _context = context;
    }

    //METODA statica ce creează sau recuperează un coș de cumpărături pentru utilizator
    //folosește sesiunea (ISession) pentru a identifica coșul fiecărui utilizator.
    public static ShoppingCart GetCart(IServiceProvider services)//primeste ca parametru colectia de servicii
    {
        ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;//Obține sesiunea utilizatorului (folosind IHttpContextAccessor); obtin acces la sesiune

        ApplicationDbContext context = services.GetService<ApplicationDbContext>() ??
               throw new Exception("Error initializing");//Obține conexiunea la baza de date (obtin acces la DbContext).

        string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();//Verifică pe baza sesiunii dacă există un CartId în sesiune. Dacă nu există, creează un GUID unic Guid.NewGuid() ca si CartId și îl salvează în sesiune. Daca exista vom lua ace avaloare din sesiune session?.GetString("CartId"). În culise,ASP.NET Core folosește un cookie pentru a asocia cereri diferite de la același utilizator,de la aceeași mașină, 

        session?.SetString("CartId", cartId);//setăm valoarea CartId-ului, dacă l-am găsit sau nu, îl adăugăm din nou la sesiune

        return new ShoppingCart(context) { ShoppingCartId = cartId };//Returnează un nou obiect( un cos de cumparaturi- ShoppingCart) cu acest CartId. Returnăm Coșul de cumpărături,trecând în DbContext și trecând în ShoppingCartId ca CartId care a fost fie acum generat, fie returnat din sesiune. Vom folosi această metodă din Program.cs.
    }

    //METODA Returneaza toate produsele din coș pentru utilizatorul curent: Verifică dacă ShoppingCartItems este deja încărcat.Dacă nu, preia din baza de date toate ShoppingCartItem asociate ShoppingCartId.Include și produsul(Product) asociat fiecărui element din coș.Returnează lista rezultată.
    public List<ShoppingCartItem> GetShoppingCartItems()
    {
        return ShoppingCartItems ??= //Operatorul ??= verifică dacă ShoppingCartItems este null:Dacă NU este null → o returnează imediat.Dacă ESTE null → încarcă datele din baza de date și le stochează în ShoppingCartItems.
                   _context.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)//Selectează din baza de date toate obiectele ShoppingCartItem unde: ShoppingCartId == ShoppingCartId → aparțin aceluiași coș de cumpărături(al utilizatorului curent).
                       .Include(s => s.Product)//Adaugă și informațiile despre produsul asociat (Product nume, pret) în fiecare ShoppingCartItem.
                       .ToList();//Execută interogarea și convertește rezultatul într-o listă (List<ShoppingCartItem>). Lista este stocată în ShoppingCartItems pentru a fi reutilizată ulterior.
    }

    //METODA Adaugă un produs în coș sau mărește cantitatea dacă există deja. Caută în baza de date dacă produsul există deja în coș. Dacă nu există, creează un nou ShoppingCartItem și îl adaugă. Dacă există, crește cantitatea (Amount). Salvează modificările (_context.SaveChanges()).
    public void AddToCart(Product product)//adaug un produs in cos; primeste ca parametru Produsul
    {
        var shoppingCartItem = _context.ShoppingCartItems.SingleOrDefault(
            s => s.Product.Id == product.Id && s.ShoppingCartId == ShoppingCartId);// Caută în baza de date (_context.ShoppingCartItems) un ShoppingCartItem care:Conține un produs(Product) cu același Id ca product.Id.Are același ShoppingCartId ca utilizatorul curent. SingleOrDefault(), care returnează:Un singur rezultat dacă există un produs care îndeplinește condiția.null dacă produsul nu există în coș.

        if (shoppingCartItem == null)//verifică dacă produsul pe care încercăm să-l adăugăm în coș nu există deja. Dacă nu există, creează un nou obiect ShoppingCartItem și îl adaugă în baza de date. shoppingCartItem este null, înseamnă că produsul nu este încă în coș, deci trebuie să-l adăugăm.
        {
            shoppingCartItem = new ShoppingCartItem//Se creează un nou ShoppingCartItem, adică un produs nou care va fi adăugat în coș.
            {
                ShoppingCartId = ShoppingCartId,//Identificatorul coșului de cumpărături al utilizatorului curent.
                Product = _context.Products.FirstOrDefault(p => p.Id == product.Id), //Se caută produsul după Id în baza de date și returnează fie produsul, fie null dacă nu există.
                Amount = 1//Se setează cantitatea inițială a produsului la 1 (prima adăugare în coș).
            };

            _context.ShoppingCartItems.Add(shoppingCartItem);//Adaugă obiectul shoppingCartItem în colecția ShoppingCartItems a bazei de date folosind _context(DbContext)
        }
        else
        {
            shoppingCartItem.Amount++;
        }

        _context.SaveChanges();
    }

    //METODA Elimină un produs din coș sau scade cantitatea. Găsește produsul în baza de date. Dacă cantitatea este mai mare de 1, o scade cu 1. Dacă cantitatea ajunge la 0, elimină produsul din coș. Salvează modificările și returnează noua cantitate a produsului.

    public int RemoveFromCart(Product product)//scot un produs din cos
    {
        var shoppingCartItem = _context.ShoppingCartItems.SingleOrDefault(
        s => s.Product.Id == product.Id && s.ShoppingCartId == ShoppingCartId);//Se caută în baza de date un obiect ShoppingCartItem care are:Product.Id == product.Id → Este produsul pe care vrem să-l eliminăm. ShoppingCartId == ShoppingCartId → Face parte din coșul utilizatorului curent.

        var localAmount = 0;//reține noua cantitate a produsului în coș după eliminare. Dacă produsul nu este găsit sau este șters complet, valoarea rămâne 0.

        if (shoppingCartItem is not null)//Dacă produsul există în coș, continuăm procesul de eliminare. Dacă produsul nu există(null), metoda nu face nimic și returnează 0.
        {
            if (shoppingCartItem.Amount > 1)//Scade cantitatea dacă este mai mare de 1
            {
                shoppingCartItem.Amount--;//Dacă produsul are mai mult de o unitate în coș, scade cantitatea cu 1.
                localAmount = shoppingCartItem.Amount;//Salvează noua cantitate în localAmount.
            }
            else
            {
                _context.ShoppingCartItems.Remove(shoppingCartItem);//Dacă produsul avea doar 1 unitate, acum ajunge la 0, așa că este eliminat complet din baza de date.


            }
        }

        _context.SaveChanges();//Toate schimbările făcute (scăderea cantității sau eliminarea produsului) sunt salvate definitiv.

        return localAmount;//Dacă produsul mai există în coș, returnează noua cantitate. Dacă produsul a fost eliminat complet, returnează 0.
    }

    //METODA Șterge toate produsele din coșul utilizatorului. Caută produsele cu ShoppingCartId și le elimină.
    public void ClearCart()
    {
        var cartItems = _context.ShoppingCartItems
            .Where(cart => cart.ShoppingCartId == ShoppingCartId);

        _context.ShoppingCartItems.RemoveRange(cartItems);
        _context.SaveChanges();
    }

    //METODA Calculează totalul prețurilor produselor din coș. Găsește toate produsele din coș. Calculează preț × cantitate pentru fiecare produs. Returnează suma totală.
    public decimal GetShoppingCartTotal()//totalul cosului de cumparaturi
    {
        var total = _context.ShoppingCartItems
            .Where(c => c.ShoppingCartId == ShoppingCartId)
            .Select(c => c.Product.Price * c.Amount)
            .Sum();

        return total;
    }
}
