using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using web.Data.EntityModels;
using web.Models;
using web.Models.InputModels;
using web.Models.ViewModels;
using web.Services;
using web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace web.Controllers
{
    public class OrderController : Controller
    {
        // OrderController owns an instance of the OrderService
        private OrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;


        // Constructor for the BookController where the _bookService is created
        public OrderController(DataContext context, UserManager<ApplicationUser> userManager)
        {
            _orderService = new OrderService(context);
            _userManager = userManager;
        }

        public IActionResult Index() {
          var userId = _userManager.GetUserId(HttpContext.User);
          var orderitems = _orderService.GetAllItemsInCart(userId);
          return View(orderitems);
        }

        [Authorize]
        public IActionResult AddToCart(int BId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            
            var book = _orderService.GetOrderItemInputModel(BId, userId);
            if(ModelState.IsValid)
            {
                _orderService.AddToCart(book);
                return RedirectToAction("Cart"); // ma breyta i ad redirecta einhvert annad
            }
            return View("Cart");
        }


        [Authorize]
        public IActionResult Cart(string orderby, Order order)
        {
          var userId = _userManager.GetUserId(HttpContext.User);
          var orderitems = _orderService.GetAllItemsInCart(userId);
          return View(orderitems);
        }
        
        public IActionResult CheckOutAddress() {
          var userId = _userManager.GetUserId(HttpContext.User);
          var addressList = _orderService.GetUserAddresses(userId);

          return View(addressList);
        }

        public IActionResult CheckOut2(AddressInputModel deliveryAddress, AddressInputModel billingAddress) {


          return View();
        }

        public IActionResult Details(int Id)
        {
            var book = _orderService.GetOrderWithId(Id);

            if (book == null)
            {
                return View("ErrorNotFound");
            }
            return View(book);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

         [HttpPost]
        public IActionResult Create(OrderInputModel inputOrder)
        {
            if(ModelState.IsValid)
            {
                _orderService.AddOrder(inputOrder);
                return RedirectToAction("Index");
            }
            return View();
        } 
        public IActionResult temp(string orderby, string genre)
        {
            var orders = new List<OrderListViewModel>();

            orders = _orderService.GetAllOrders();

            if (orderby == "date-asc")
            {
                orders = _orderService.OrderByDate(orders);
            }
            else if(orderby == "date-desc")
            {
                orders = _orderService.OrderByDateDesc(orders);
            }
            else if(orderby == "amount-asc")
            {
                orders = _orderService.OrderByAmount(orders);
            }
            else if(orderby == "amount-desc")
            {
                orders = _orderService.OrderByAmountDesc(orders);
            }
            else
            {
                orders = _orderService.GetAllOrders();
            }      
            return View(orders);
        }
/*

        [HttpGet]
        public IActionResult Search(string searchString)
        {
            var foundBooks = _bookService.SearchResults(searchString);
            return View(foundBooks);
        }
         */


    }
}

