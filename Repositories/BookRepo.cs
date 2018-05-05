using System.Collections.Generic;
using web.Data;
using System.Linq;
using web.Models.ViewModels;
using System;

namespace web.Repositories
{
    public class BookRepo
    {
        // BookRepo owns a private instance of the databae
        private DataContext _db;

        // Constructor to initialize the database
        public BookRepo()
        {
            _db = new DataContext();
        }


        // Function that returns a list of all the books in a database
        public List<BookListViewModel> GetAllBooks()
        {
            var books = (from b in _db.Books
                         select new BookListViewModel
                         {
                             Id = b.Id,
                             Name = b.Name,
                             AuthorId = b.AuthorId,
                             ImageId = b.ImageId,
                             Rating = b.Rating,
                             Price = b.Price,
                             Discount = b.Discount
                         }).ToList();

            return books;
        }

        // Function that returns a book with specified id
        public BookDetailViewModel GetBookWithId(int Id)
        {
            var book = (from b in _db.Books
                        where b.Id == Id
                        select new BookDetailViewModel
                        {
                            Id = b.Id,
                            Name = b.Name,
                            AuthorId = b.AuthorId,
                            PublisherId = b.PublisherId,
                            ImageId = b.ImageId,
                            Year = b.Year,
                            ISBN = b.ISBN,
                            Language = b.Language
                        }).SingleOrDefault();

            return book;
        }
    }
}