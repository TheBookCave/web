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

namespace web.Controllers
{
    public class BookController : Controller
    {
        // BookController owns an instance of the BookService
        private BookService _bookService;

        // Constructor for the BookController where the _bookService is created
        public BookController()
        {
            _bookService = new BookService();
        }

        public IActionResult Index()
        {
            var books = _bookService.GetAllBooks();

            return View(books);
        }

        public IActionResult Details(int Id)
        {
            var book = _bookService.GetBookWithId(Id);

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
        public IActionResult Create(BookInputModel inputBook)
        {
            if(ModelState.IsValid)
            {
                _bookService.AddBook(inputBook);
                return RedirectToAction("Index");
            }
            return View();
        } 

    }
}
