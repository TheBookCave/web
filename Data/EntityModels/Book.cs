namespace web.Data.EntityModels
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public string ImageUrl { get; set; }
        public int Year { get; set; }
        public string ISBN { get; set; }
        public string Language { get; set; }
        public int Quantity { get; set;}
        public double Price { get; set; }
        public double Discount { get; set; }
    }
}