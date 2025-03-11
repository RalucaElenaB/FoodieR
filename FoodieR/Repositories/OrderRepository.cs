using FoodieR.Data;
using FoodieR.Models;
using FoodieR.Models.DbObject;

namespace FoodieR.Repositories;

public class OrderRepository
{
    private readonly ApplicationDbContext _context;//_context → Obiectul bazei de date (ApplicationDbContext) folosit pentru a salva comenzile si fiind readonly=nu trebuie să fie modificat după ce este inițializat.
    private readonly ShoppingCart _shoppingCart;//_shoppingCart → Dependență care gestionează coșul de cumpărături.

    //Constructorul primește aceste dependențe prin Dependency injection(design pattern) și le stochează pentru a fi folosite în CreateOrder.
    public OrderRepository(ApplicationDbContext context, ShoppingCart shoppingCart)//Constructorul primește două obiecte → context și shoppingCart=furnizate de sistemul de Dependency Injection al ASP.NET Core
    {
        _context = context;//_context → Folosit pentru a comunica cu baza de date (adică, pentru a salva comenzile).
        _shoppingCart = shoppingCart;//_shoppingCart → Folosit pentru a accesa produsele din coșul de cumpărături.
    }

    //CREATE
    public void AddOrder(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
    }

    public IEnumerable<Order> GetOrders()
    {
        return _context.Orders.ToList();
    }


    //READ one Order
    public Order GetOrderById(int id)
    {
        return _context.Orders.FirstOrDefault(o => o.Id == id);
    }

    //UPDATE
    public void UpdateOrder(Order order)
    {
        _context.Orders.Update(order);
        _context.SaveChanges();
    }

    //DELETE
    public void DeleteOder(int id)
    {
        var order = _context.Orders.FirstOrDefault(o => o.Id == id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }      
    }
    //METODA creează o nouă comandă (Order) pe baza produselor din coșul de cumpărături (ShoppingCart).
    public void CreateOrder(Order order)//primesc o comanda pe care trebuie sa o salvez in baza de date
    {
        order.CreateDate = DateTime.Now;//Se setează data curentă a comenzii.
        List<ShoppingCartItem>? shoppingCartItems = _shoppingCart.GetShoppingCartItems();//Se preia lista produselor din coș.
        order.TotalAmount = _shoppingCart.GetShoppingCartTotal();//Se calculează suma totală a comenzii.
        order.OrderLines = new List<OrderLine>();

        //detaliile comenzii
        foreach (var item in shoppingCartItems)
        {
            var orderLine = new OrderLine//Se creează o listă de OrderLines pentru fiecare produs din coș.
            {
                Product = item.Product,//Setează produsul aferent liniei comenzii(si il adauga la OrderLine)
                Amount = item.Amount,//Setează suma totală pentru acel produs.
                Quantity = 0,
                Price = item.Product.Price,//Setează prețul unitar al produsului.
            };

            order.OrderLines.Add(orderLine);//se adaugă noua linie de comandă la lista OrderLines
        }

        _context.Orders.Add(order);//order este adăugată la baza de date
        _context.SaveChanges();//salvează toate modificările în baza de date
    }
}
