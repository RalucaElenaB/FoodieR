using FoodieR.Models.DbObject;
using FoodieR.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodieR.Controllers
{
    public class ProductController : Controller
    {
        public readonly ProductRepository _productRepository;
        public readonly CategoryRepository _categoryRepository;

        public ProductController(ProductRepository productRepository,
            CategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

      
        // GET: ProductController
        public ActionResult Index()
        {
            var products = _productRepository.GetProducts();
            return View(products);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            var product = _productRepository.GetProductById(id);
            return View(product);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            var categories = _categoryRepository.GetCategories();

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");

            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Product product = new Product
                {
                    Name = collection["Name"],
                    Price = decimal.Parse(collection["Price"]),
                    CategoryId = int.Parse(collection["CategoryId"]),

                };
                _productRepository.AddProduct(product);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            var product = _productRepository.GetProductById(id);
            var categories = _categoryRepository.GetCategories().Where(c => c.Id == product.CategoryId);
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            
            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var product = _productRepository.GetProductById(id);
                product.Name = collection["Name"];
                product.Price = decimal.Parse(collection["Price"]);
                product.CategoryId = int.Parse(collection["CategoryId"]);
                _productRepository.UpdateProduct(product);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            var product = _productRepository.GetProductById(id);
            return View(product);
           
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _productRepository.DeleteProduct(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
