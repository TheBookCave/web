using System.Collections.Generic;

namespace web.Models.ViewModels
{
    // BookListViewModel for the customer
    public class BookListViewModel
    {
        public int Id { get; set; } // To link to Book details
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double Rating { get; set; }
    }
}