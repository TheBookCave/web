using System;
using System.Collections.Generic;

namespace web.Data.EntityModels
{
    public class Rating
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string CustomerId { get; set; }
        public int RatingValue { get; set; }
        public string Comment { get; set; }
        public DateTime RatingDate { get; set; }
    }
}