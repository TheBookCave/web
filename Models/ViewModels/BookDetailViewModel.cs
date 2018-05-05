namespace web.Models.ViewModels
{
    // BookDetailViewModel for the customer
    public class BookDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public int ImageId { get; set; }
        public int Year { get; set; }
        public string ISBN { get; set; }
        public string Language { get; set; }

        // Price

        // Rating

        // Discount
    }
}