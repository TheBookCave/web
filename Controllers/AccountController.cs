using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using web.Services;

using Microsoft.AspNetCore.Identity;
using web.Models;
using web.Models.ViewModels;
using web.Models.InputModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;

using web.Data.EntityModels;
using web.Data;

using web.Repositories;

namespace web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private DataContext _context;
        private BookService _bookService;
        private AuthenticationDbContext _aContext;
        private OrderService _orderService;
        
        private readonly IHostingEnvironment _appEnvironment;
       // var RoleManager = new RoleManager<IdentityRole>

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IHostingEnvironment appEnvironment, DataContext context, AuthenticationDbContext aContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _appEnvironment = appEnvironment;
            _aContext = aContext;
            _context = context;
            _bookService = new BookService(context, aContext);
            _orderService = new OrderService(context);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);

            var _accountViewModel = new AccountViewModel();
            if(user.FavoriteBookId > 0)
            {
                _accountViewModel.FavoriteBookName = _bookService.GetBookWithId(user.FavoriteBookId).Name;
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
            
            return View(_accountViewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit()
        {
            
            var _userEditInputModel = new UserEditInputModel();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            _userEditInputModel.AllBooks = _bookService.GetAllBooks();
            _userEditInputModel.AllAddresses = _orderService.GetUserAddresses(userId);
            return View(_userEditInputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditInputModel model)
        {
            if(!ModelState.IsValid) { 
                return View(); 
            }

            //Get user
            var user = await _userManager.GetUserAsync(HttpContext.User);


            // Save image
            var pic = model.UserPhoto;
            if(pic != null)
            {
                

                //Create path for image, images/profilepics/{id}{filename}
                var relPath = Path.Combine(Path.Combine("images","profilepics"),user.Id + Path.GetFileName(pic.FileName));
                var fileName = Path.Combine(_appEnvironment.WebRootPath, relPath);
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

            return RedirectToAction("Index", "Account");
        }

        
        [Authorize]
        [HttpGet]
        public IActionResult CreateAddress()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAddress(AddressListViewModel model)
        {
            if(!ModelState.IsValid) { 
                return View(); 
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            model.CustomerId = user.Id;
            var _addressRepo = new AddressRepo(_context);
            _addressRepo.AddAddress(model);
            return RedirectToAction("Index", "Account");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid) { return View(); }
            
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
            var result = await _userManager.CreateAsync(user, model.Password);

            if(! await _roleManager.RoleExistsAsync("Staff"))
            {
                var staffRole = new IdentityRole();
                staffRole.Name = "Staff";
                await _roleManager.CreateAsync(staffRole);
                await _userManager.AddToRoleAsync(user, "Staff");
            }

            if(result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, new Claim("Name", $"{model.FirstName} {model.LastName}"));
                await _signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Account");
            }
            return View();
        }

        [Authorize(Roles = "Staff")]
        [HttpGet]
        public IActionResult RegisterStaff()
        {
            return View();
        }

        [Authorize(Roles = "Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterStaff(RegisterViewModel model)
        {
            if(!ModelState.IsValid) { return View(); }
            
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Staff");
                await _userManager.AddClaimAsync(user, new Claim("Name", $"{model.FirstName} {model.LastName}"));

                return RedirectToAction("Index", "Staff");
            }
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(!ModelState.IsValid) { return View(); }
            
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.Remember, false);

            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Account");
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }


        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
