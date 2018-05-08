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

namespace web.Controllers
{
    public class OrderController : Controller
    {
        // OrderController owns an instance of the OrderService
        private OrderService _orderService;

        // Constructor for the BookController where the _bookService is created
        public OrderController(DataContext context)
        {
            _orderService = new OrderService(context);
        }

        public IActionResult Index(string orderby, string genre)
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

