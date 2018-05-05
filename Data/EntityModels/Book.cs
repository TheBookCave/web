namespace web.Data.EntityModels
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public int ImageId { get; set; }
        public int Year { get; set; }
        public string ISBN { get; set; }
        public string Language { get; set; }
        public int Quantity { get; set;}
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Rating { get; set; }
    }
}