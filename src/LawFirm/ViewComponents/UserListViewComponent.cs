using LawFirm.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LawFirm.ViewComponents
{
    public class UserListViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public UserListViewComponent (ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(_context.Users.ToList());
        }
    }
}
