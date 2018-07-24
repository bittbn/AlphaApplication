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
    public class ReservationController : Controller
    {
        private AlphaContext _db = new AlphaContext();

        [Authorize(Roles = "Офисный менеджер")]
        public async Task<ActionResult> Index()
        {
            var reservations = _db.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .Where(r => r.Allow == false)
                .OrderByDescending(r => r.Id);

            return View(await reservations.ToListAsync());
        }

        public async Task<ActionResult> Create(int? userId, int? roomId)
        {
            if (userId == null || roomId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = await _db.Users.FindAsync(userId);
            Room room = await _db.Rooms.FindAsync(roomId);

            if (user == null || room == null)
            {
                return HttpNotFound();
            }

            ViewBag.User = user;
            ViewBag.Room = room;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Date,TimeStart,TimeEnd,UserId,RoomId")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _db.Reservations.Add(reservation);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Rooms");
            }

            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Allow(int? id, bool allow)
        {
            Reservation reservation = await _db.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            reservation.Allow = allow;
            _db.Entry(reservation).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = await _db.Reservations.FindAsync(id);
            _db.Reservations.Remove(reservation);
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
