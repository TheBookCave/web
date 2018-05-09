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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace web.Controllers
{
    public class BookController : Controller
    {
        // BookController owns an instance of the BookService
        private BookService _bookService;
        private readonly UserManager<ApplicationUser> _userManager;

        // Constructor for the BookController where the _bookService is created
        public BookController(DataContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _bookService = new BookService(context);
            _userManager = userManager;
        }

        public IActionResult Index(string orderby, string searchString2, string genreFilter)
        {

            var books = _bookService.GetAllBooks();

            if (!String.IsNullOrEmpty(searchString2))
            {
                books = _bookService.SearchResults(searchString2);
            }

            if(!String.IsNullOrEmpty(genreFilter) && genreFilter != "0")
            {
                books = _bookService.FilterGenre(books, genreFilter);
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

            if(books.Count == 0)
            {
                return View("ErrorNotFound");
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

        [Authorize(Roles = "Staff")]
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

        [Authorize(Roles = "Staff")]
        [HttpPost]
        public IActionResult Create(BookInputModel inputBook)
        {
            if(ModelState.IsValid)
            {
                _bookService.AddBook(inputBook);
                return RedirectToAction("Index", "Staff");
            }
            return View();
        } 

        [Authorize]
        [HttpGet]
        public IActionResult RateBook(int BId, string BookName)
        {

            var book = new RatingInputModel
            {
                BookId = BId,
                CustomerId = "",
                RatingValue = 0,
                Comment = "",
                RatingDate = DateTime.Now
            };
            
            return View(book);
        }

        [Authorize]
        [HttpPost]
        public IActionResult RateBook(RatingInputModel rating)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            rating.CustomerId = userId; 
            rating.RatingDate = DateTime.Now;


            if(ModelState.IsValid)
            {
                _bookService.AddRating(rating);
                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize(Roles = "Staff")]
        [HttpPost]
        public IActionResult CreateGenre(GenreInputModel inputGenre)
        {
            if(ModelState.IsValid)
            {
                _bookService.AddGenre(inputGenre);
                return RedirectToAction("Index", "Staff");
            }
            return View();
        } 

        [Authorize(Roles = "Staff")]
        [HttpGet]
        public IActionResult CreateGenre()
        {
            return View();
        }


        [Authorize(Roles = "Staff")]
        [HttpPost]
        public IActionResult CreateAuthor(AuthorInputModel inputAuthor)
        {
            if(ModelState.IsValid)
            {
                _bookService.AddAuthor(inputAuthor);
                return RedirectToAction("Index", "Staff");
            }
            return View();
        } 

        [Authorize(Roles = "Staff")]
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
                return RedirectToAction("Index", "Staff");
            }
            return View();
        } 

        [Authorize(Roles = "Staff")]
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
