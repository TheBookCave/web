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
        // AccountService owns a private instance of OrderRepo and AddressRepo
        private OrderRepo _orderRepo;
        private AddressRepo _addressRepo;
        private BookRepo _bookRepo;

        private AuthenticationDbContext _aContext;


        // Constructor for OrderService that creates the _orderRepo
        public AccountService(DataContext context, AuthenticationDbContext aContext)
        {
            _orderRepo = new OrderRepo(context);
            _addressRepo = new AddressRepo(context);
            _bookRepo = new BookRepo(context, aContext);
            _aContext = aContext;
        }

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
                _accountViewModel.PrimaryAddressStreet = _addressRepo.GetAddressById(user.PrimaryAddressId).StreetAddress;//user.PrimaryAddressId.ToString();
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

        public void AddAddressByUser(AddressListViewModel model, ApplicationUser user)
        {
            model.CustomerId = user.Id;
            _addressRepo.AddAddress(model);
        }

        public string GetPrimaryAddressByUser(ApplicationUser user)
        {
            var address = _addressRepo.GetAddressById(user.PrimaryAddressId);
            return address.StreetAddress + ", " + address.LastName;
        }

        public async Task<bool>  UpdateUserChangedValues(ApplicationUser user, UserEditInputModel model, string rootPath)
        {
            
            // Save image
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
