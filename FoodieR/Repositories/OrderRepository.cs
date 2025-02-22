using FoodieR.Data;
using FoodieR.Models.DbObject;
using Microsoft.EntityFrameworkCore;

namespace FoodieR.Repositories;

public class OrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
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

  
}
