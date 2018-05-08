using System.Collections.Generic;
using web.Data;
using System.Linq;
using web.Models.ViewModels;
using System;
using web.Models.InputModels;
using web.Data.EntityModels;


namespace web.Repositories
{
    public class AuthorRepo
    {
        private DataContext _db;

        public AuthorRepo(DataContext context) {
            _db = context;
        }

// Function that returns a list of all the genres in a database
        public List<AuthorListViewModel> GetAllAuthors()
        {
            var authors = (from a in _db.Authors
                         select new AuthorListViewModel
                         {
                             Id = a.Id,
                             Name = a.Name
                         }).ToList();
            return authors;
        }


        public void AddAuthor(AuthorInputModel inputAuthor)
        {
            var newAuthor = new Author()
            {
                //Id
                Name = inputAuthor.Name
            };

            _db.Authors.Add(newAuthor);
            _db.SaveChanges();
        }
    }
}