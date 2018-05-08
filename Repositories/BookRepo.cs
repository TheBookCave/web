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
        public BookRepo(DataContext context)
        {
            _db = context;
        }

        // Function that return all available Genres with their ID so that they can be clickable
        public List<GenreListViewModel> GetAllGenres()
        {
            var genres = (from g in _db.Genres
                         select new GenreListViewModel
                         {
                             Id = g.Id,
                             Name = g.Name
                         }).ToList();
            return genres;
        }

        // Function that returns BookGenreViewModel for all books
         public IQueryable<BookGenreViewModel> GetGenresForAllBooks()
         {
            // Join Books, BookGenres and Genres tables together to a flat view
            var allBookGenres = ( from b in _db.Books
                           join bg in _db.BookGenres on b.Id equals bg.BookId
                           join g in _db.Genres on bg.GenreId equals g.Id into view
                           from x in view.DefaultIfEmpty()
                           select new
                           {
                               BookId = b.Id,
                               GenreId = x.Id,
                               GenreName = x.Name
                           });

            // Group the view by BookId and create a BookGenreViewModel that returns the bookId and list of all the Genres
            var genresForAllBooks = (from b in allBookGenres group b.GenreName by b.BookId into g
                         select new BookGenreViewModel{
                             BookId = g.Key,
                             BookGenres = g.ToList()
                         });
                           
            return genresForAllBooks;
         }

//--------------------------------------------------------------------------------------------------------------------------------------------
        // Function that returns a Linq Query of all the books in a database
        public IQueryable<BookListViewModel> GetAllBooksLinqQuery()
        {
            var ratings = GetAllAverageRatings();

            var allGenres = GetAllGenres();

            var bookGenres = GetGenresForAllBooks();

            var books = (from b in _db.Books
                         join at in _db.Authors on b.AuthorId equals at.Id
                         join g in bookGenres on b.Id equals g.BookId
                         join r in ratings on b.Id equals r.BookId into a
                         from c in a.DefaultIfEmpty(new RatingViewModel() {BookId = b.Id, AverageRating = 0})
                         select new BookListViewModel
                         {
                             Id = b.Id,
                             Name = b.Name,
                             Author = at.Name,
                             ImageUrl = b.ImageUrl,
                             Price = b.Price,
                             Discount = b.Discount,
                             Rating = c.AverageRating,
                             Genres = g.BookGenres,
                             AllGenres = allGenres
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