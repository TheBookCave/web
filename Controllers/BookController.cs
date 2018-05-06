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

        public IActionResult Index(string orderby)
        {
            var books = new List<BookListViewModel>();
            if (orderby == "name-asc")
            {
                books = _bookService.GetAllBooksOrderedByName();
            }
            else if(orderby == "name-desc")
            {
                books = _bookService.GetAllBooksOrderedByNameDesc();
            }
            else if(orderby == "id")
            {
                books = _bookService.GetAllBooks();
            }
            else if(orderby == "price-asc")
            {
                books = _bookService.GetAllBooksOrderedByPrice();
            }
            else if(orderby == "price-desc")
            {
                books = _bookService.GetAllBooksOrderedByPriceDesc();
            }
            else if(orderby == "rating-asc")
            {
                books = _bookService.GetAllBooksOrderedByRating();
            }
            else if(orderby == "rating-desc")
            {
                books = _bookService.GetAllBooksOrderedByRatingDesc();
            }
            else
            {
                books = _bookService.GetAllBooks();
            }      
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

            var allGenres = new BookInputModel {
                AllGenres = _bookService.GetAllGenres()
            };
            return View(allGenres);
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

         [HttpPost]
        public IActionResult CreateGenre(GenreInputModel inputGenre)
        {
            if(ModelState.IsValid)
            {
                _bookService.AddGenre(inputGenre);
                return RedirectToAction("Index");
            }
            return View();
        } 

        [HttpGet]
        public IActionResult CreateGenre()
        {
            return View();
        }



         [HttpPost]
        public IActionResult CreateAuthor(AuthorInputModel inputAuthor)
        {
            if(ModelState.IsValid)
            {
                _bookService.AddAuthor(inputAuthor);
                return RedirectToAction("Index");
            }
            return View();
        } 

        [HttpGet]
        public IActionResult CreateAuthor()
        {
            return View();
        }


        public IActionResult CreatePublisher(PublisherInputModel inputPublisher)
        {
            if(ModelState.IsValid)
            {
                _bookService.AddPublisher(inputPublisher);
                return RedirectToAction("Index");
            }
            return View();
        } 

        [HttpGet]
        public IActionResult CreatePublisher()
        {
            return View();
        }

    }
}
