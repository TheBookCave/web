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

        // Function that returns a Linq Query of all the books in a database

        public IQueryable<BookListViewModel> GetAllBooksLinqQuery()
        {
            var ratings = GetAllAverageRatings();


            var books = (from b in _db.Books
                         join r in ratings on b.Id equals r.BookId into a
                         from c in a.DefaultIfEmpty(new RatingViewModel() {BookId = b.Id, AverageRating = 0})
                         select new BookListViewModel
                         {
                             Id = b.Id,
                             Name = b.Name,
                             AuthorId = b.AuthorId,
                             ImageUrl = b.ImageUrl,
                             Price = b.Price,
                             Discount = b.Discount,
                             Rating = c.AverageRating
                         });
            return books;
        }

        // Function that returns the list of Books
        public List<BookListViewModel> GetAllBooks()
        {
            var books = GetAllBooksLinqQuery().ToList();
            return books;
        }

        // Function that calculates average rating for all books
        public List<RatingViewModel> GetAllAverageRatings()
        {
            var averageRatings = ( from r in _db.Ratings
                                   group r by r.BookId into rating
                                   select new RatingViewModel
                                   {
                                       BookId = rating.Key,
                                       AverageRating = rating.Average(x => x.RatingValue)
                                   }).ToList();
        
            return averageRatings;
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