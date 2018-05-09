using System;
using System.Collections.Generic;

namespace web.Models.InputModels
{
    public class RatingInputModel
    {
        public int BookId { get; set; }
        public int CustomerId { get; set; }
        public int RatingValue { get; set; }
        public string Comment { get; set; }
        public DateTime RatingDate { get; set; }
    }
}