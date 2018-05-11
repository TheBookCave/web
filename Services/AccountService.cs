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

namespace web.Services
{
    public class AccountService
    {
        // AccountService owns a private instance of OrderRepo and AddressRepo
        private OrderRepo _orderRepo;
        private AddressRepo _addressRepo;
        private BookRepo _bookRepo;

        // Constructor for OrderService that creates the _orderRepo
        public AccountService(DataContext context, AuthenticationDbContext aContext)
        {
            _orderRepo = new OrderRepo(context);
            _addressRepo = new AddressRepo(context);
            _bookRepo = new BookRepo(context, aContext);
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
                _accountViewModel.PrimaryAddressStreet = user.PrimaryAddressId.ToString();
            } 
            else
            {
                _accountViewModel.PrimaryAddressStreet = "None";
            }
            if(_accountViewModel.UserPhotoLocation != null)
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

        
    }
}
