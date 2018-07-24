using AlphaApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Net;

namespace AlphaApplication.Controllers
{
    public class AccountController : Controller
    {
        private AlphaContext _db = new AlphaContext();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                user = await _db.Users.FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Index", "Rooms");
                }
                else                
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");                
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                user = await  _db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);

                if (user == null)
                {
                    _db.Users.Add(new User
                    {
                        Name = model.Name,
                        Surname = model.Surname,
                        Login = model.Login,
                        Password = model.Password,
                        RoleId = 2
                    });
                    await _db.SaveChangesAsync();

                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Index", "Rooms");
                }
                else
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
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