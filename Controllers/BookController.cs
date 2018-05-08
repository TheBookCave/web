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
    public class BookController : Controller
    {
        // BookController owns an instance of the BookService
        private BookService _bookService;

        // Constructor for the BookController where the _bookService is created
        public BookController(DataContext context)
        {
            _bookService = new BookService(context);
        }

        public IActionResult Index(string orderby, string searchString2)
        {

            var books = _bookService.GetAllBooks();

            if (!String.IsNullOrEmpty(searchString2))
            {
                books = _bookService.SearchResults(searchString2);
            }

            if (orderby == "name-asc")
            {
                books = _bookService.OrderByName(books);
            }
            else if(orderby == "name-desc")
            {
                books = _bookService.OrderByNameDesc(books);
            }
            else if(orderby == "price-asc")
            {
                books = _bookService.OrderByPrice(books);
            }
            else if(orderby == "price-desc")
            {
                books = _bookService.OrderByPriceDesc(books);
            }
            else if(orderby == "rating-asc")
            {
                books = _bookService.OrderByRating(books);
            }
            else if(orderby == "rating-desc")
            {
                books = _bookService.OrderByRatingDesc(books);
            }
    
            return View(books);
        }

        public IActionResult Top10()
        {
            var books = _bookService.GetTop10Books();
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

            var inputModel = new BookInputModel {
                AllGenres = _bookService.GetAllGenres(),
                AllAuthors = _bookService.GetAllAuthors(),
                AllPublishers = _bookService.GetAllPublishers()
            };
            return View(inputModel);
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
                return RedirectToAction("Create");
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
                return RedirectToAction("Create");
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
                return RedirectToAction("Create");
            }
            return View();
        } 

        [HttpGet]
        public IActionResult CreatePublisher()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Search(string searchString)
        {
            var foundBooks = _bookService.SearchResults(searchString);
            return View(foundBooks);
        }


    }
}
