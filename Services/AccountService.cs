using System.Collections.Generic;
using web.Models.InputModels;
using web.Models.ViewModels;
using web.Repositories;
using System.Linq;
using System;
using web.Data;
using web.Data.EntityModels;
using web.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace web.Services
{
    public class AccountService
    {
        private OrderRepo _orderRepo;
        private AddressRepo _addressRepo;
        private BookRepo _bookRepo;
        private AuthenticationDbContext _aContext;


        // Constructor for AccountService
        public AccountService(DataContext context, AuthenticationDbContext aContext)
        {
            // Initialize repositories
            _orderRepo = new OrderRepo(context);
            _addressRepo = new AddressRepo(context);
            _bookRepo = new BookRepo(context, aContext);

            // Shared AuthenticationDbContext
            // Exception to the 3 layer design due to using the Identity Framework
            _aContext = aContext;
        }

        // Get account view model for a specific user and substitute Null/0 values that have no reference
        public AccountViewModel GetAccountViewModelByUser(ApplicationUser user)
        {
            var _accountViewModel = new AccountViewModel();
            if(user.FavoriteBookId > 0)
            {
                _accountViewModel.FavoriteBookName = _bookRepo.GetBookWithId(user.FavoriteBookId).Name;
            } 
            else
            {
                _accountViewModel.FavoriteBookName = "None";
            }
            if(user.PrimaryAddressId > 0)
            {
                _accountViewModel.PrimaryAddressStreet = this.GetAddressString(user.PrimaryAddressId);
            } 
            else
            {
                _accountViewModel.PrimaryAddressStreet = "None";
            }
            if(user.UserPhotoLocation != null)
            {
                _accountViewModel.UserPhotoLocation = user.UserPhotoLocation;
            }
            else
            {
               _accountViewModel.UserPhotoLocation = "images\\profilepics\\default.jpg";
            }

            _accountViewModel.FirstName = user.FirstName;
            _accountViewModel.LastName = user.LastName;
            _accountViewModel.Email = user.Email;
            return _accountViewModel;
        }

        // Add an address for a specific user
        public void AddAddressByUser(AddressListViewModel model, ApplicationUser user)
        {
            model.CustomerId = user.Id;
            _addressRepo.AddAddress(model);
        }

        // Returns main parts of an address in one string for display purpouses
        public string GetAddressString(int id)
        {
            var address = _addressRepo.GetAddressById(id);
            return address.StreetAddress + ", " +  address.Country + ", " + address.City + ", " + address.FirstName + " " + address.LastName;
        }

        // Update user values for input values that are not null
        public async Task<bool>  UpdateUserChangedValues(ApplicationUser user, UserEditInputModel model, string rootPath)
        {
            
            // Save image if uploaded
            var pic = model.UserPhoto;
            if(pic != null)
            {
                //Create path for image, images/profilepics/{id}{filename}
                var relPath = Path.Combine(Path.Combine("images","profilepics"),user.Id + Path.GetFileName(pic.FileName));
                var fileName = Path.Combine(rootPath, relPath);
                var stream = new FileStream(fileName, FileMode.Create);
                await pic.CopyToAsync(stream);
                stream.Close();
                
                //Save to User table
                user.UserPhotoLocation = relPath;   
            }
            

            // Save other values if changed
            if(model.FirstName != null) {
                user.FirstName = model.FirstName;
            }
            if(model.LastName != null) {
                user.LastName = model.LastName;
            }
            if(model.FavoriteBookId > 0) {
                user.FavoriteBookId = model.FavoriteBookId;
            }
            if(model.PrimaryAddressId > 0) {
                user.PrimaryAddressId = model.PrimaryAddressId;
            }


            var result = await _aContext.SaveChangesAsync();
            return true;
        } 


    }
}
