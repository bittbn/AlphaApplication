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
    [Authorize]
    public class RoomsController : Controller
    {
        private AlphaContext _db = new AlphaContext();
        
        public async Task<ActionResult> Index()
        {
            var rooms = _db.Rooms
                .Include(r => r.Reservations)
                .OrderByDescending(r => r.Id);

            return View(await rooms.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Room room = await _db.Rooms.FindAsync(id);
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Login == User.Identity.Name);

            var reservations = await _db.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .Where(r => r.RoomId == id && r.Allow == true)
                .OrderByDescending(r => r.Id)
                .ToListAsync();

            if (room == null || user == null|| reservations == null)
            {
                return HttpNotFound();
            }

            ViewBag.Room = room;
            ViewBag.User = user;

            return View(reservations);
        }

        [Authorize(Roles = "Офисный менеджер")]
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description,Seats,Board,Projector")] Room room)
        {
            if (ModelState.IsValid)
            {
                _db.Rooms.Add(room);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(room);
        }

        [Authorize(Roles = "Офисный менеджер")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = await _db.Rooms.FindAsync(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,Seats,Board,Projector")] Room room)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(room).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(room);
        }

        [Authorize(Roles = "Офисный менеджер")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = await _db.Rooms.FindAsync(id);
            if (room == null)
            {
                return HttpNotFound();
            }

            _db.Rooms.Remove(room);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
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
