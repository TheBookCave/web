using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace web.Models.InputModels
{
    public class RatingInputModel
    {
        public int BookId { get; set; }
        public string CustomerId { get; set; }
        
        [Required(ErrorMessage ="Rating is required")]      // Problem line
        [RegularExpression("[0-5]")]                        // Problem line
        public int RatingValue { get; set; }
        public string Comment { get; set; }
        public DateTime RatingDate { get; set; }
    }
}