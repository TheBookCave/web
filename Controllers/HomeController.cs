using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using web.Data;
using web.Models;
using web.Services;

namespace web.Controllers
{
    public class HomeController : Controller
    {
        private BookService _bookService;

        public HomeController(DataContext context, AuthenticationDbContext aContext)
        {
            _bookService = new BookService(context, aContext);
        }
        public IActionResult Index()
        {
            var books = _bookService.GetRecentAdditionsBooks();
            return View(books);
        }
    }
}
