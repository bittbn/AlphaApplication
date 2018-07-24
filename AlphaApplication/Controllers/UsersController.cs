using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AlphaApplication.Models;

namespace AlphaApplication.Controllers
{
    [Authorize(Roles = "Офисный менеджер")]
    public class UsersController : Controller
    {
        private AlphaContext _db = new AlphaContext();
        
        public async Task<ActionResult> Index()
        {
            var users = _db.Users
                .Include(u => u.Role)
                .OrderByDescending(u => u.Id);

            return View(await users.ToListAsync());
        }
        
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        
        
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(_db.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Surname,Login,Password,RoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(user).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(_db.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
