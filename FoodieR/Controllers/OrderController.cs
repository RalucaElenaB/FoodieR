using FoodieR.Models.DbObject;
using FoodieR.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FoodieR.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderRepository _orderRepository;
        private readonly UserRepository _userRepository;

        public OrderController(OrderRepository orderRepository, UserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
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
    }
}
