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
        



        [HttpGet]
        public IActionResult CheckOutAddress() {
          var userId = _userManager.GetUserId(HttpContext.User);
          var addressList = _orderService.GetUserAddresses(userId);
          var openorder = _orderService.GetOpenOrder(userId);

          var order = new OrderInputModel() {
            AllUserAddresses = addressList
          };

          return View(order);
        }

        [HttpPost]
        public IActionResult CheckOutAddress(OrderInputModel order) { // 2 addresses, [0] is delivery, [1] is billing
          var userId = _userManager.GetUserId(HttpContext.User);
          var addressList = _orderService.GetUserAddresses(userId);
          var openorder = _orderService.GetOpenOrder(userId);

          order.Id = openorder.Id;
          order.CustomerId = openorder.CustomerId;
          order.Status = openorder.Status;
          order.OrderDate = openorder.OrderDate;
          order.ShippingDate = openorder.ShippingDate;
          order.TrackingNumber = openorder.TrackingNumber;
          order.PurchaseAmount = openorder.PurchaseAmount;
          order.AllUserAddresses = addressList;

            if(ModelState.IsValid)
            {
                _orderService.ChangeOrderAddress(order);
                return RedirectToAction("CheckOutReview");

            }
          return View("CheckOutAddress", order);
        }

      [HttpGet]
      public IActionResult CheckOutReview() {
      var userId = _userManager.GetUserId(HttpContext.User);
      var openorder = _orderService.GetOpenOrder(userId);
      var addressesUsed = _orderService.GetAddressesOnOrder(openorder.Id);

      var ordertoconfirm = new OrderConfirmationViewModel() {
        Order = openorder,
        Addresses = addressesUsed
      };

      return View(ordertoconfirm);
      }

      [HttpPost]
        public IActionResult CheckOutReview(OrderConfirmationViewModel ordertoconfirm) {


        /*  var userId = _userManager.GetUserId(HttpContext.User);
          
                    var confirmation = new OrderConfirmationViewModel() {
            Order = _orderService.GetOrderWithId(order.Id),
            Addresses = []
          };
           */

          //confirmation.Order.ShippingAddressId = chosenaddresses[0].Id;
          //confirmation.Order.BillingAddressId = chosenaddresses[1].Id;
          return View(ordertoconfirm);
        }

      [HttpPost]
        public IActionResult CheckOutConfirmed(OrderConfirmationViewModel confirmed) {
           if(ModelState.IsValid)
            {
                _orderService.CloseOrder(confirmed);
                return RedirectToAction("Index");
            }

          return View(confirmed);
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

        [Authorize]
        [HttpPost]
        public IActionResult AjaxRemoveOrderItem(int orderItemId)
        {
            if (ModelState.IsValid && orderItemId != 0)
            {
                _orderService.DeleteOrderItem(orderItemId);
                return Ok();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AjaxChangeOrderItemQuantity(int orderItemId, bool increment)
        {
            if (ModelState.IsValid && orderItemId != 0)
            {
                if (increment) 
                {
                    _orderService.IncrementOrderItemQuantity(orderItemId);
                }
                else 
                {
                    _orderService.DecrementOrderItemQuantity(orderItemId);
                }
                return Ok();
            }
            return BadRequest();
        }
    }
}

