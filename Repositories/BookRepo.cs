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
                             Price = b.Price,
                             Discount = b.Discount
                         }).ToList();

            return books;
        }

        // Function that returns a list of all books ordered by the book name
        public List<BookListViewModel> GetAllBooksOrderedByName()
        {
            var books = (from b in _db.Books
                         select new BookListViewModel
                         {
                             Id = b.Id,
                             Name = b.Name,
                             AuthorId = b.AuthorId,
                             ImageUrl = b.ImageUrl,
                             Price = b.Price,
                             Discount = b.Discount
                         }).OrderBy(x => x.Name).ToList();

            return books;            
        }

        // Function that returns a list of all books ordered by the book name descending
        public List<BookListViewModel> GetAllBooksOrderedByNameDesc()
        {
            var books = (from b in _db.Books
                         select new BookListViewModel
                         {
                             Id = b.Id,
                             Name = b.Name,
                             AuthorId = b.AuthorId,
                             ImageUrl = b.ImageUrl,
                             Price = b.Price,
                             Discount = b.Discount
                         }).OrderByDescending(x => x.Name).ToList();

            return books;            
        }

        // Function that returns a list of all books ordered by the book price descending
        public List<BookListViewModel> GetAllBooksOrderedByPriceDesc()
        {
            var books = (from b in _db.Books
                         select new BookListViewModel
                         {
                             Id = b.Id,
                             Name = b.Name,
                             AuthorId = b.AuthorId,
                             ImageUrl = b.ImageUrl,
                             Price = b.Price,
                             Discount = b.Discount
                         }).OrderByDescending(x => x.Price).ToList();

            return books;            
        }

        // Function that returns a list of all books ordered by the book price
        public List<BookListViewModel> GetAllBooksOrderedByPrice()
        {
            var books = (from b in _db.Books
                         select new BookListViewModel
                         {
                             Id = b.Id,
                             Name = b.Name,
                             AuthorId = b.AuthorId,
                             ImageUrl = b.ImageUrl,
                             Price = b.Price,
                             Discount = b.Discount
                         }).OrderBy(x => x.Price).ToList();

            return books;            
        }

        // Function that returns a book with specified id
        public BookDetailViewModel GetBookWithId(int Id)
        {
            var rating = 0.0;
            var ratings = ( from r in _db.Ratings 
                           where r.BookId == Id
                           select r.RatingValue ).ToList();

            if (ratings.Count != 0)
            {
                rating = ratings.Average();
            }

            var book = (from b in _db.Books
                        join a in _db.Authors on b.AuthorId equals a.Id
                        join p in _db.Publishers on b.PublisherId equals p.Id
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
            };
            
            _db.Books.Add(newBook);
            _db.SaveChanges();

            foreach (var genre in inputBook.Genres) {
                var _bookGenre = new BookGenre() {
                    //Id
                    BookId = newBook.Id,
                    GenreId = genre
                };
                _db.BookGenres.Add(_bookGenre);
            }
            _db.SaveChanges();
        }

    }
}