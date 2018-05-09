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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using web.Data.EntityModels;

namespace web.Controllers
{
    
    [Authorize(Roles = "Staff")]
    public class StaffController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public StaffController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }
}
