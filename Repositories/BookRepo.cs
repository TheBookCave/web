using System.Collections.Generic;
using web.Models.ViewModels;

namespace web.Repositories
{
    public class BookRepo
    {
        // Function that returns a list of all the books in a database
        public List<BookListViewModel> GetAllBooks()
        {
            var books = new List<BookListViewModel>
            {
                new BookListViewModel {Id = 1, Name = "Book 1", AuthorId = 1, PublisherId = 1, ImageId = 1, Year = 2000, ISBN = "ISBN1", Language = "English"},
                new BookListViewModel {Id = 2, Name = "Book 2", AuthorId = 2, PublisherId = 2, ImageId = 2, Year = 2000, ISBN = "ISBN2", Language = "English"},
                new BookListViewModel {Id = 3, Name = "Book 3", AuthorId = 3, PublisherId = 3, ImageId = 3, Year = 2000, ISBN = "ISBN3", Language = "English"}
            };

            return books;
        }
    }
}