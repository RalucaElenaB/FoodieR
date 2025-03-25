using FoodieR.Data;
using FoodieR.Models.DbObject;
using Microsoft.EntityFrameworkCore;

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

        //READ = filtrare
        public IEnumerable<Product> GetProducts(string searchProduct)
        {
            return _context.Products
                .Include(product => product.Category)
                .Where(product => product.Name.Contains(searchProduct))
                .ToList();
        }

        //READ
        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.Include(c => c.Category).FirstOrDefaultAsync(p => p.Id == id);
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
