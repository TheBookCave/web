using System.Collections.Generic;

namespace web.Models.ViewModels
{
    // BookDetailViewModel for the customer
    public class BookDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public int PublisherId { get; set; }
        public string PublisherName { get; set; }
        public string ImageUrl { get; set; }
        public int Year { get; set; }
        public string ISBN { get; set; }
        public string Language { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double Rating { get; set; }
        public List<RatingCommentViewModel> Comments { get; set; }
    }
}