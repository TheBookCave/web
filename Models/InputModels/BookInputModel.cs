namespace web.Models.InputModels
{
    public class BookInputModel
    {
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
    }
}