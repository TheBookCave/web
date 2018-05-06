using System.Collections.Generic;
using web.Data.EntityModels;

namespace web.Models.InputModels
{
    public class BookInputModel
    {
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public string ImageUrl { get; set; }
        public int Year { get; set; }
        public string ISBN { get; set; }
        public string Language { get; set; }
        public int Quantity { get; set;}
        public double Price { get; set; }
        public double Discount { get; set; }
        public List<Genre> Genres { get; set;}
    }
}