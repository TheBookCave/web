using System.Collections.Generic;
using web.Models.InputModels;
using web.Models.ViewModels;
using web.Repositories;
using System.Linq;
using System;
using web.Data;

namespace web.Services
{
    public class BookService
    {
        // BookService owns a private instance of BookRepo
        private BookRepo _bookRepo;
        private GenreRepo _genreRepo;
        private AuthorRepo _authorRepo;
        private PublisherRepo _publisherRepo;

        // Constructor for BookService that creates the _bookRepo
        public BookService(DataContext context, AuthenticationDbContext acontext)
        {
            _bookRepo = new BookRepo(context, acontext);
            _genreRepo = new GenreRepo(context);
            _authorRepo = new AuthorRepo(context);
            _publisherRepo = new PublisherRepo(context);
        }

        // Function that return a list of all the books
        public List<BookListViewModel> GetAllBooks()
        {
            var books = _bookRepo.GetAllBooks();
            return books;
        }

        // Function that return a list of all the books in alphabetic order
        public List<BookListViewModel> OrderByName(List<BookListViewModel> methodBooks)
        {
            var books = methodBooks.OrderBy(x => x.Name).ToList();
            return books;
        }

        // Function that return a list of all the books in alphabetic order desc.
        public List<BookListViewModel> OrderByNameDesc(List<BookListViewModel> methodBooks)
        {
            var books = methodBooks.OrderByDescending(x => x.Name).ToList();
            return books;
        }

        // Function that return a list of all the books ordered by price
        public List<BookListViewModel> OrderByPrice(List<BookListViewModel> methodBooks)
        {
            var books = methodBooks.OrderBy(x => x.Price).ToList();
            return books;
        }

        // Function that return a list of all the books ordered by price desc
        public List<BookListViewModel> OrderByPriceDesc(List<BookListViewModel> methodBooks)
        {
            var books = methodBooks.OrderByDescending(x => x.Price).ToList();
            return books;
        }

        // Function that return a list of all the books ordered by rating desc
        public List<BookListViewModel> OrderByRatingDesc(List<BookListViewModel> methodBooks)
        {
            var books = methodBooks.OrderByDescending(x => x.Rating).ToList();
            return books;
        }

        // Function that return a list of all the books ordered by rating
        public List<BookListViewModel> OrderByRating(List<BookListViewModel> methodBooks)
        {
            var books = methodBooks.OrderBy(x => x.Rating).ToList();
            return books;
        }

        // Function that return a list of all the books ordered by rating
        public List<BookListViewModel> GetTop10Books()
        {
            var books = _bookRepo.GetAllBooksLinqQuery().OrderByDescending(x => x.Rating).Take(10).ToList();
            return books;
        }

        // Function that returns a list of the most recently added books
        public List<BookListViewModel> GetRecentAdditionsBooks()
        {
            var books = _bookRepo.GetAllBooksLinqQuery().OrderByDescending(x => x.Id).Take(5).ToList();
            return books;
        }

        // Function that return a book with specified ID
        public BookDetailViewModel GetBookWithId(int Id)
        {
            var book = _bookRepo.GetBookWithId(Id);
            return book;
        }

        // Function that searches books
        public List<BookListViewModel> SearchResults(string searchString)
        {
            if(searchString == null) 
            {
                searchString = "";
            }
            searchString = searchString.ToLower();
            
            var books = _bookRepo.GetAllBooksLinqQuery().Where( a => a.Author.ToLower().Contains(searchString) || a.Name.ToLower().Contains(searchString)).ToList();

            return books;
        }

        // Function that filters based on genre
        public List<BookListViewModel> FilterGenre(List<BookListViewModel> books, string genreFilter)
        {
            if(genreFilter == null || genreFilter == "0")
            {
                return books;
            }

            books = books.Where( a => string.Join(",", a.Genres).Contains(genreFilter)).ToList();

            return books;
        }

        public void AddBook(BookInputModel inputBook)
        {
            _bookRepo.AddBook(inputBook);
        }


        public void AddGenre(GenreInputModel inputGenre)
        {
            _genreRepo.AddGenre(inputGenre);
        }

        public List<GenreListViewModel> GetAllGenres()
        {
            var genres = _genreRepo.GetAllGenres();
            return genres;
        }

        public List<AuthorListViewModel> GetAllAuthors()
        {
            var authors = _authorRepo.GetAllAuthors();
            return authors;
        }

        public List<PublisherListViewModel> GetAllPublishers()
        {
            var publishers = _publisherRepo.GetAllPublishers();
            foreach (var p in publishers) {
                Console.WriteLine(p);
            }
            return publishers;
        }

        public void AddAuthor(AuthorInputModel inputAuthor)
        {
            _authorRepo.AddAuthor(inputAuthor);
        }

        public void AddRating(RatingInputModel rating)
        {
            _bookRepo.AddRating(rating);
        }


        public void AddPublisher(PublisherInputModel inputPublisher)
        {
            _publisherRepo.AddPublisher(inputPublisher);
        }

        
        public BookInputModel CreateNewBookInputModel()
        {
           var book = new BookInputModel
            {  
                AllAuthors = GetAllAuthors(),
                AllGenres = GetAllGenres(),
                AllPublishers = GetAllPublishers()
            };
            return book;
        } 

    }
}