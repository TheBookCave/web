namespace web.Models.ViewModels
{
    // BookListViewModel for the customer
    public class BookListViewModel
    {
        public int Id { get; set; } // To link to Book details
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public int ImageId { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Rating { get; set; }

    }
}