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

    public IActionResult Index()
    {
      var userId = _userManager.GetUserId(HttpContext.User);
      var orderitems = _orderService.GetAllItemsInCart(userId);
      return View(orderitems);
    }


    [Authorize]
    public IActionResult AddToCart(int BId)
    {
      var userId = _userManager.GetUserId(HttpContext.User);


      var book = _orderService.GetOrderItemInputModel(BId, userId);
      if (ModelState.IsValid)
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



    [Authorize]
    [HttpGet]
    public IActionResult CheckOutReview()
    {
      var userId = _userManager.GetUserId(HttpContext.User);
      var openorder = _orderService.GetOpenOrder(userId);
      var addressesUsed = _orderService.GetAddressesOnOrder(openorder.Id);
      var orderItems = _orderService.GetAllItemsInOrder(openorder.Id);

      var ordertoconfirm = new OrderConfirmationViewModel()
      {
        Order = openorder,
        Addresses = addressesUsed,
        OrderItems = orderItems
      };

      return View(ordertoconfirm);
    }



    [Authorize]

    [HttpGet]
    public IActionResult CheckOutAddress()
    {
      var userId = _userManager.GetUserId(HttpContext.User);
      var addressList = _orderService.GetUserAddresses(userId);
      var openorder = _orderService.GetOpenOrder(userId);

      var order = new OrderInputModel()
      {
        AllUserAddresses = addressList
      };

      return View(order);
    }


    [Authorize]

    [HttpPost]
    public IActionResult CheckOutAddress(OrderInputModel order)
    { // 2 addresses, [0] is delivery, [1] is billing
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

      if (ModelState.IsValid)
      {
        _orderService.ChangeOrderAddress(order);
        return RedirectToAction("CheckOutReview");

      }
      return View("CheckOutAddress", order);
    }



    [Authorize]
    [HttpPost]
    public IActionResult CheckOutReview(OrderConfirmationViewModel ordertoconfirm)
    {
      return View(ordertoconfirm);
    }


    [Authorize]
    public IActionResult CheckOutConfirmed()
    {
      var userId = _userManager.GetUserId(HttpContext.User);
      _orderService.CloseOrder(userId);

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
      if (ModelState.IsValid)
      {
        _orderService.AddOrder(inputOrder);
        return RedirectToAction("Index");
      }
      return View();
    }

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

