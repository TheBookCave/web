using System.Collections.Generic;

namespace web.Models.ViewModels
{
    // BookListViewModel for the customer
    public class BookListViewModel
    {
        public int Id { get; set; } // To link to Book details
        public string Name { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double Rating { get; set; }
        public List<string> Genres { get; set; }
        public List<GenreListViewModel> AllGenres {get; set;}

    }
}