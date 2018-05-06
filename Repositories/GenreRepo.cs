using System.Collections.Generic;
using web.Data;
using System.Linq;
using web.Models.ViewModels;
using System;
using web.Models.InputModels;
using web.Data.EntityModels;


namespace web.Repositories
{
    public class GenreRepo
    {
        private DataContext _db;

        public GenreRepo() {
            _db = new DataContext();
        }

// Function that returns a list of all the genres in a database
        public List<GenreListViewModel> GetAllGenres()
        {
            var genres = (from g in _db.Genres
                         select new GenreListViewModel
                         {
                             Id = g.Id,
                             Name = g.Name
                         }).ToList();
            return genres;
        }


        public void AddGenre(GenreInputModel inputGenre)
        {
            var newGenre = new Genre()
            {
                //Id
                Name = inputGenre.Name
            };

            _db.Genres.Add(newGenre);
            _db.SaveChanges();
        }

    }
}