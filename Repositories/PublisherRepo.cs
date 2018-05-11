using System.Collections.Generic;
using web.Data;
using System.Linq;
using web.Models.ViewModels;
using System;
using web.Models.InputModels;
using web.Data.EntityModels;


namespace web.Repositories
{
  public class PublisherRepo
  {
    private DataContext _db;

    public PublisherRepo(DataContext context)
    {
      _db = context;
    }

    // Function that returns a list of all the genres in a database
    public List<PublisherListViewModel> GetAllPublishers()
    {
      var publishers = (from p in _db.Publishers
                        select new PublisherListViewModel
                        {
                          Id = p.Id,
                          Name = p.Name
                        }).ToList();
      return publishers;
    }


    public void AddPublisher(PublisherInputModel inputPublisher)
    {
      var newPublisher = new Publisher()
      {
        //Id
        Name = inputPublisher.Name
      };

      _db.Publishers.Add(newPublisher);
      _db.SaveChanges();
    }
  }
}