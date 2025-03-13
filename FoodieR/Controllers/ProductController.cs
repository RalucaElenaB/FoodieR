using FoodieR.Models;
using FoodieR.Models.DbObject;
using FoodieR.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodieR.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductRepository _productRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly ReviewRepository _reviewRepository;
        private const string _adminId = "2791fad0-c69d-4691-876a-dbff73644de3";
        private readonly UserManager<IdentityUser> _userManager;

        public ProductController(ProductRepository productRepository,
            CategoryRepository categoryRepository, ReviewRepository reviewRepository, 
            UserManager<IdentityUser> userManager)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _reviewRepository = reviewRepository;
            _userManager = userManager;
        }

      
        // GET: ProductController
        public async Task<ActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);//extrag userul logat

            var products = _productRepository.GetProducts();

            var productsViewModel = products.Select(product => new ProductViewModel
            {
                ProductId = product.Id,
                Category = product.Category,
                Name = product.Name,
                CategoryId = product.CategoryId,
                Price = product.Price,
                IsAdmin = user?.Id == _adminId,
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
                Description = product.Description,
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

                var productsViewModelWithAllProducts = allProducts.Select(product => new ProductViewModel
                {
                    ProductId = product.Id,
                    Category = product.Category,
                    Name = product.Name,
                    CategoryId = product.CategoryId,
                    Price = product.Price,
                });
                return View("Index", productsViewModelWithAllProducts); // Afișează rezultatele în Index.cshtml
            }

            // Filtrare produse în funcție de termenul de căutare
            var filteredProducts = _productRepository.GetProducts(searchProduct);
            ViewData["CurrentFilter"] = searchProduct; // Păstrează termenul de căutare

            var productsViewModel = filteredProducts.Select(product => new ProductViewModel
            {
                ProductId = product.Id,
                Category = product.Category,
                Name = product.Name,
                CategoryId = product.CategoryId,
                Price = product.Price,
            });

            return View("Index", productsViewModel); // Afișează rezultatele în Index.cshtml
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
                    Description = collection["Description"]

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

                product.Description = collection["Description"];

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
