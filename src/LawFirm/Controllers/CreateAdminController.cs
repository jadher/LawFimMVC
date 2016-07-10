using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LawFirm.Data;
using Microsoft.AspNetCore.Identity;
using LawFirm.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LawFirm.Controllers
{
    public class CreateAdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateAdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {

            var user = _context.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);

            if (!(await _roleManager.RoleExistsAsync("Administrator")))
            {
                var role = new IdentityRole();
                role.Name = "Administrator";
                await _roleManager.CreateAsync(role);
            }

            var addResult = await _userManager.AddToRoleAsync(user, "Administrator");
            if (!addResult.Succeeded)
            {
                return View("Error");
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("~/Index");
        }
    }
}