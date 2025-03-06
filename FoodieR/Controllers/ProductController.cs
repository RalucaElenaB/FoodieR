using FoodieR.Models;
using FoodieR.Models.DbObject;
using FoodieR.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace FoodieR.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductRepository _productRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly ReviewRepository _reviewRepository;

        public ProductController(ProductRepository productRepository,
            CategoryRepository categoryRepository, ReviewRepository reviewRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _reviewRepository = reviewRepository;
        }

      
        // GET: ProductController
        public ActionResult Index()
        {
            var products = _productRepository.GetProducts();
            var productsViewModel = products.Select(product => new ProductViewModel
            {
                ProductId = product.Id,
                Category = product.Category,
                Name = product.Name,
                CategoryId = product.CategoryId,
                Price = product.Price,
            });

            return View(productsViewModel);
        }

        // GET: ProductController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var product = await _productRepository.GetProductById(id);
            var reviews = _reviewRepository.GetReviewsByProductId(product.Id);

            var productViewModel = new ProductViewModel
            {
                ProductId = product.Id,
                Category = product.Category,
                Name = product.Name,
                CategoryId = product.CategoryId,
                Price = product.Price,
                Reviews = reviews.Select(review => ReviewViewModel.FromEntity(review))
            };


            return View(productViewModel);
        }

        // GET: ProductController/Filter
        public ActionResult Filter(string searchProduct)
        {
            if (string.IsNullOrEmpty(searchProduct))
            {
                var allProducts = _productRepository.GetProducts();
                return View("Index", allProducts); // Afișează rezultatele în Index.cshtml
            }

            // Filtrare produse în funcție de termenul de căutare
            var filteredProducts = _productRepository.GetProducts(searchProduct);
            ViewData["CurrentFilter"] = searchProduct; // Păstrează termenul de căutare

            return View("Index", filteredProducts); // Afișează rezultatele în Index.cshtml
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
        public async Task<ActionResult> Edit(int id)
        {
            var product = await _productRepository.GetProductById(id);
            var categories = _categoryRepository.GetCategories().Where(c => c.Id == product.CategoryId);

     
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            
            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                var product = await _productRepository.GetProductById(id);
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
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _productRepository.GetProductById(id);
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
