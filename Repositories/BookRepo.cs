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

 //------------------------------------------------------------------------------------------- Virkar örugglega ekki eðlilega
 // Vantar að útbúa View á þessu formati:
 // |  Book ID  | List<GenreViewModel> |       þar sem GenreViewModel inniheldur Genre ID og Genre Name eða bara List<string> þar sem það er nafnið á genre.
         public List<BookGenreViewModel> GetGenresForAllBooks()
         {
             var newList = new List<string>();

            var genres = ( from b in _db.Books
                           join bg in _db.BookGenres on b.Id equals bg.BookId
                           join g in _db.Genres on bg.GenreId equals g.Id into view
                           from x in view.DefaultIfEmpty()
                           select new
                           {
                               BookId = x.Id,
                               BookGenres = view.SelectMany(y => y.Name).ToList()
                           }).ToList();
            
            var result = genres.SelectMany(x => x.BookGenres).ToList();
                           
            return null;
            //     from bg in _db.BookGenres
            //               join g in _db.Genres on bg.GenreId equals g.Id into a
            //               from b in a.DefaultIfEmpty(new BookGenreViewModel() {BookId = bg.BookId, BookGenres = null})
            //               select new 
            // )
            // return genres;
         }

//--------------------------------------------------------------------------------------------------------------------------------------------
        // Function that returns a Linq Query of all the books in a database
        public IQueryable<BookListViewModel> GetAllBooksLinqQuery()
        {
            var ratings = GetAllAverageRatings();

            var allGenres = GetAllGenres();

            var books = (from b in _db.Books
                         join at in _db.Authors on b.AuthorId equals at.Id
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
                             Genres = null,
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