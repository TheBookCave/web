using System;
using System.Collections.Generic;

namespace web.Models.ViewModels
{
    public class RatingCommentViewModel
    {
        //public int BookId { get; set; }
        //public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        
        public int RatingValue { get; set; }
        public string Comment { get; set; }
        public DateTime RatingDate { get; set; }
    }
}