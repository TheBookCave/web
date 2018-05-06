using System.Collections.Generic;
using web.Models.ViewModels;
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
        public List<int> Genres { get; set; }
        public List<GenreListViewModel> AllGenres { get; set; }
        public List<AuthorListViewModel>AllAuthors { get; set; }
    }
}