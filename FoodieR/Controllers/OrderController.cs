using FoodieR.Models;
using FoodieR.Models.DbObject;
using FoodieR.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodieR.Controllers
{
    public class OrderController : Controller
    {
        //aduc dependintele necesare
        private readonly OrderRepository _orderRepository;
        private readonly UserRepository _userRepository;
        private readonly ShoppingCart _shoppingCart;
        private readonly UserManager<IdentityUser> _userManager;


        public OrderController(OrderRepository orderRepository, UserRepository userRepository, ShoppingCart shoppingCart, UserManager<IdentityUser> userManager)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _shoppingCart = shoppingCart;
            _userManager = userManager;

        }


        // GET: OrderController
        public ActionResult Index()
        {
            string userName = User?.Identity?.Name ?? "undefined";
            var user = _userRepository.GetUserByUserName(userName);

            var orders = _orderRepository.GetOrders();
            return View(orders);
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            var order = _orderRepository.GetOrderById(id);
            return View(order);
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            string userName = User?.Identity?.Name ?? "undefined";

            var user = _userRepository.GetUserByUserName(userName);
            ViewBag.User = user;


            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Order order = new Order
                {
                    CreateDate = DateTime.Parse(collection["CreateDate"]),
                    TotalAmount = decimal.Parse(collection["TotalAmount"]),
                };
                _orderRepository.AddOrder(order);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            var order = _orderRepository.GetOrderById(id);
            return View(order);
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var order = _orderRepository.GetOrderById(id);
                order.CreateDate = DateTime.Parse(collection["Date Time"]);
                order.TotalAmount = decimal.Parse(collection["Total Amount"]);
                _orderRepository.UpdateOrder(order);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            var order = _orderRepository.GetOrderById(id);
            return View(order);
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _orderRepository.DeleteOder(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //formular de finalizare a platii
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(Order order, IFormCollection form)
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            if (_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "your cart is empty, ad some products first.");
            }
            var user = await _userManager.GetUserAsync(User);
            order.Customer = user;
            if (ModelState.IsValid)
            {
                _orderRepository.CreateOrder(order);
                _shoppingCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }


            return View(order);
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Thanks for your order. You'll soon enjoy your products.";
            return View();
        }
    }
}
