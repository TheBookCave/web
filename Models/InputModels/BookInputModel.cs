using System.Collections.Generic;
using web.Models.ViewModels;
using web.Data.EntityModels;
using System.ComponentModel.DataAnnotations;

namespace web.Models.InputModels
{
    public class BookInputModel
    {
        [Required]
        public string Name { get; set; }
        public int AuthorId { get; set; }
        [Required]
        public string Description { get; set; }
        public int PublisherId { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        [RegularExpression("[0-9]{4}")]
        public int Year { get; set; }
        [Required]
        [RegularExpression("[0-9]{13}")]        
        public string ISBN { get; set; }
        public string Language { get; set; }
        public int Quantity { get; set;}
        public double Price { get; set; }
        public double Discount { get; set; }
        public List<int> Genres { get; set; }
        public List<GenreListViewModel> AllGenres { get; set; }
        public List<AuthorListViewModel> AllAuthors { get; set; }
        public List<PublisherListViewModel> AllPublishers { get; set; }

    }
}