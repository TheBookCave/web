using System.Collections.Generic;
using web.Models.ViewModels;
using web.Data.EntityModels;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace web.Models.InputModels
{
    public class EditBookInputModel
    {
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please select a book from the dropdown list")]
        public int BookId { get; set; }
        [Required]
        public string Name { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "Author Name is required")]
        public int AuthorId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Publisher Name is required")]
        public int PublisherId { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Year is required")]
        [RegularExpression("[0-9]{4}", ErrorMessage = "Year has to be a four digit number")]
        public int Year { get; set; }
        [Required]
        [RegularExpression("[0-9]{13}", ErrorMessage = "ISBN13 number is required")]        
        public string ISBN { get; set; }
        [Required]
        public string Language { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$")]
        public int Quantity { get; set;}
        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double Discount { get; set; }
        public List<int> Genres { get; set; }
        public List<GenreListViewModel> AllGenres { get; set; }
        public List<AuthorListViewModel> AllAuthors { get; set; }
        public List<PublisherListViewModel> AllPublishers { get; set; }
        public List<BookListViewModel> AllBooks { get; set; }

    }
}