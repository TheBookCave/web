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

namespace web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private AuthenticationDbContext _aContext;
        
        private readonly IHostingEnvironment _appEnvironment;
       // var RoleManager = new RoleManager<IdentityRole>

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IHostingEnvironment appEnvironment, AuthenticationDbContext aContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _appEnvironment = appEnvironment;
            _aContext = aContext;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            //var userid = _userManager.GetUserId(HttpContext.User);
            //ApplicationUser user = _userManager.FindByIdAsync(userid).Result;
           // _userManager.GetClaimsAsync(user)
           
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit()
        {
            //var userid = _userManager.GetUserId(HttpContext.User);
            //ApplicationUser user = _userManager.FindByIdAsync(userid).Result;
            //Console.WriteLine(user.UserName);
           // _userManager.GetClaimsAsync(user)
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditInputModel model)
        {
            if(!ModelState.IsValid) { 
                return View(); 
            }

            var pic = model.UserPhoto;
            if(pic != null)
            {
                //Get user
                var user = await _userManager.GetUserAsync(HttpContext.User);

                //Create path for image, images/profilepics/{id}{filename}
                var relPath = Path.Combine(Path.Combine("images","profilepics"),user.Id + Path.GetFileName(pic.FileName));
                var fileName = Path.Combine(_appEnvironment.WebRootPath, relPath);
                var stream = new FileStream(fileName, FileMode.Create);
                await pic.CopyToAsync(stream);
                stream.Close();
                
                
                //Save as a claim for the user
                //await _userManager.RemoveClaimAsync(user, HttpContext.User.Claims.FirstOrDefault(x => x.Type == "ProfilePic"));
                //await _userManager.AddClaimAsync(user, new Claim("ProfilePic", $"{relPath}"));

                //Save to User table
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserPhotoLocation = relPath;
                user.FavoriteBookId = model.FavoriteBookId;
                user.PrimaryAddressId = model.PrimaryAddressId;

                var result = await _aContext.SaveChangesAsync();
            }

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
