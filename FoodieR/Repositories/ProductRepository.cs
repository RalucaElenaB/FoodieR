using FoodieR.Data;
using FoodieR.Models.DbObject;
using Microsoft.EntityFrameworkCore;
using System.CodeDom;
using System.Linq;

namespace FoodieR.Repositories
{
    public class ProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //CREATE
        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        //READ
        public IEnumerable<Product> GetProducts()
        {
            return _context.Products.Include(c => c.Category).ToList();
        }

        //READ
        public Product GetProductById(int id)
        {
            return _context.Products.Include(c => c.Category).FirstOrDefault(p => p.Id == id);
        }

        //UPDATE
        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        //DELETE
         public void DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
