using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LawFirm.Data;
using LawFirm.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LawFirm.Controllers
{
    [Authorize]
    public class ProcessosController : Controller
    {
        private readonly ApplicationDbContext _context;      


        public ProcessosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
          
        }

        // GET: Processos
        public async Task<IActionResult> Index()
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == HttpContext.User.Identity.Name);
            List<Processo> processos = new List<Processo>();

            ViewBag.Name = user.RealName;
            if (user == null)
            {
                return View(await _context.Processo.ToListAsync());
            }
            else
            {
                foreach(var item in _context.Processo)
                {
                    if(item.UserId == user.Id)
                    {
                        processos.Add(item);
                    }
                }
                if (processos == null)
                {
                    //todo: rever isso
                    return View(await _context.Processo.ToListAsync());
                }
                else return View(processos);
            }
            

        }

        // GET: Processos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processo = await _context.Processo.SingleOrDefaultAsync(m => m.ProcessoId == id);
            if (processo == null)
            {
                return NotFound();
            }

            return View(processo);
        }

        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> DetailsAdmin(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processo = await _context.Processo.SingleOrDefaultAsync(m => m.ProcessoId == id);
            if (processo == null)
            {
                return NotFound();
            }

            return View(processo);
        }

        // GET: Processos/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewBag.UserList = _context.Users.ToList();
            return View();
        }

        // POST: Processos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> Create([Bind("ProcessoId,Comment,NumeroUnico,Status,UserId")] Processo processo)
        {
            //var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == HttpContext.User.Identity.Name);
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == processo.UserId);
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                   processo.UserId = user.Id;
                    processo.UserName = user.RealName;
                }
                                
                _context.Add(processo);                
                
                await _context.SaveChangesAsync();
                return RedirectToAction("All");
            }
            return View(processo);
        }

        // GET: Processos/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processo = await _context.Processo.SingleOrDefaultAsync(m => m.ProcessoId == id);
            if (processo == null)
            {
                return NotFound();
            }
            ViewBag.UserList = _context.Users.ToList();
            return View(processo);
        }

        // POST: Processos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProcessoId,Comment,NumeroUnico,Status,UserId,UserName")] Processo processo)
        {
            if (id != processo.ProcessoId)
            {
                return NotFound();
            }
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == processo.UserId);
            if (ModelState.IsValid)
            {
                try
                {
                    if (user != null)
                    {
                        processo.UserId = user.Id;
                        processo.UserName = user.RealName;
                    }
                    _context.Update(processo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessoExists(processo.ProcessoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("All");
            }
            return View(processo);
        }

        // GET: Processos/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processo = await _context.Processo.SingleOrDefaultAsync(m => m.ProcessoId == id);
            if (processo == null)
            {
                return NotFound();
            }

            return View(processo);
        }

        // POST: Processos/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var processo = await _context.Processo.SingleOrDefaultAsync(m => m.ProcessoId == id);
            _context.Processo.Remove(processo);
            await _context.SaveChangesAsync();
            return RedirectToAction("All");
        }

        private bool ProcessoExists(int id)
        {
            return _context.Processo.Any(e => e.ProcessoId == id);
        }

        
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> All()
        {
            return View("AdminList", await _context.Processo.ToListAsync());
        }
    }
}
