using web.Data;
using web.Data.EntityModels;

namespace web.Repositories
{
    public class PublisherRepo
    {
        // PublisherRepo owns a private instance of the databae
        private DataContext _db;

        // Constructor to initialize the database
        public PublisherRepo()
        {
            _db = new DataContext();
        }

    }
}