using System.Collections.Generic;
using web.Models.ViewModels;
using web.Repositories;

namespace web.Services
{
    public class BookService
    {
        // BookService owns a private instance of BookRepo
        private BookRepo _bookRepo;

        // Constructor for BookService that creates the _bookRepo
        public BookService()
        {
            _bookRepo = new BookRepo();
        }

        // Function that return a list of all the books
        public List<BookListViewModel> GetAllBooks()
        {
            var books = _bookRepo.GetAllBooks();
            return books;
        }

        // Function that return a book with specified ID
        public BookDetailViewModel GetBookWithId(int Id)
        {
            var book = _bookRepo.GetBookWithId(Id);
            return book;
        }
    }
}