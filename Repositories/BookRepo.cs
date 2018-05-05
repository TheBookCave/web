using System.Collections.Generic;
using web.Data;
using System.Linq;
using web.Models.ViewModels;

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
                             PublisherId = b.PublisherId,
                             ImageId = b.ImageId,
                             Year = b.Year,
                             ISBN = b.ISBN,
                             Language = b.Language
                         }).ToList();

            return books;
        }
    }
}