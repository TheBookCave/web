using System.Collections.Generic;
using web.Data;
using System.Linq;
using web.Models.ViewModels;
using System;
using web.Models.InputModels;
using web.Data.EntityModels;


namespace web.Repositories
{
    public class AddressRepo
    {
        private DataContext _db;

        public AddressRepo(DataContext context) {
            _db = context;
        }

// Function that returns a list of all the genres in a database
        public List<AddressListViewModel> GetAllAddresses()
        {
            var addresses = (from p in _db.Addresses
                         select new AddressListViewModel
                         {
                             Id = p.Id,
                             City = p.City,
                             Country = p.Country,
                             CustomerId = p.CustomerId,
                             FirstName = p.FirstName,
                             LastName = p.LastName,
                             PhoneNumber = p.PhoneNumber,
                             StreetAddress = p.StreetAddress,
                             ZipCode = p.ZipCode
                         }).ToList();
            return addresses;
        }

        public AddressListViewModel GetAddressById(int _id)
        {
            var address = (from p in _db.Addresses
                         where p.Id == _id
                         select new AddressListViewModel
                         {
                             Id = p.Id,
                             City = p.City,
                             Country = p.Country,
                             CustomerId = p.CustomerId,
                             FirstName = p.FirstName,
                             LastName = p.LastName,
                             PhoneNumber = p.PhoneNumber,
                             StreetAddress = p.StreetAddress,
                             ZipCode = p.ZipCode
                         }).FirstOrDefault();
            return address;
        }

        public void AddAddress(AddressInputModel inputAddress, string userId)
        {
            var newAddress = new Address()
            {
                //Id
                City = inputAddress.City,
                Country = inputAddress.Country,
                CustomerId = userId,
                FirstName = inputAddress.FirstName,
                LastName = inputAddress.LastName,
                PhoneNumber = inputAddress.PhoneNumber,
                StreetAddress = inputAddress.StreetAddress,
                ZipCode = inputAddress.ZipCode
            };

            _db.Addresses.Add(newAddress);
            _db.SaveChanges();
        }
    }
}