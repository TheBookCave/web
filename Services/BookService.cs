using System.Collections.Generic;
using web.Models.InputModels;
using web.Models.ViewModels;
using web.Repositories;
using System.Linq;

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
        public BookService()
        {
            _bookRepo = new BookRepo();
            _genreRepo = new GenreRepo();
            _authorRepo = new AuthorRepo();
            _publisherRepo = new PublisherRepo();
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

        // Function that return a book with specified ID
        public BookDetailViewModel GetBookWithId(int Id)
        {
            var book = _bookRepo.GetBookWithId(Id);
            return book;
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

        public void AddAuthor(AuthorInputModel inputAuthor)
        {
            _authorRepo.AddAuthor(inputAuthor);
        }


        public void AddPublisher(PublisherInputModel inputPublisher)
        {
            _publisherRepo.AddPublisher(inputPublisher);
        }
    }
}