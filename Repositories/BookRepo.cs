using System.Collections.Generic;
using web.Data;
using System.Linq;
using web.Models.ViewModels;
using System;
using web.Models.InputModels;
using web.Data.EntityModels;

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
                             ImageUrl = b.ImageUrl,
                             Rating = b.Rating,
                             Price = b.Price,
                             Discount = b.Discount
                         }).ToList();

            return books;
        }

        // Function that returns a book with specified id
        public BookDetailViewModel GetBookWithId(int Id)
        {
            var rating = ( from r in _db.Ratings 
                           where r.BookId == Id
                           select r.RatingValue ).Average();

            var book = (from b in _db.Books
                        join a in _db.Authors on b.AuthorId equals a.Id
                        join p in _db.Publishers on b.PublisherId equals p.Id
                        join r in _db.Ratings on b.Id equals r.BookId
                        where b.Id == Id
                        select new BookDetailViewModel
                        {
                            Id = b.Id,
                            Name = b.Name,
                            AuthorId = b.AuthorId,
                            AuthorName = a.Name,
                            PublisherId = b.PublisherId,
                            PublisherName = p.Name,
                            ImageUrl = b.ImageUrl,
                            Year = b.Year,
                            ISBN = b.ISBN,
                            Language = b.Language,
                            Price = b.Price,
                            Discount = b.Discount,
                            Rating = rating
                        }).FirstOrDefault();

            return book;
        }

        public void AddBook(BookInputModel inputBook)
        {
            var newBook = new Book()
            {
                //Id
                Name = inputBook.Name,
                AuthorId = inputBook.AuthorId,
                PublisherId = inputBook.PublisherId,
                ImageUrl = inputBook.ImageUrl,
                Year = inputBook.Year,
                ISBN = inputBook.ISBN,
                Language = inputBook.Language,
                Quantity = inputBook.Quantity,
                Price = inputBook.Price, 
                Discount = inputBook.Discount,
                Rating = 0
            };

            _db.Books.Add(newBook);
            _db.SaveChanges();
        }
    }
}