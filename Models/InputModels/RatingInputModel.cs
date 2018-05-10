using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace web.Models.InputModels
{
    public class RatingInputModel
    {
        public int BookId { get; set; }

        public string BookName { get; set; }
        public string CustomerId { get; set; }
        
    	[Required(ErrorMessage ="Rating is required")]
        [RegularExpression("[0-5]", ErrorMessage="Rating must be between 0 and 5")] 
        public int RatingValue { get; set; }
        public string Comment { get; set; }
        public DateTime RatingDate { get; set; }
    }
}