using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GovEventer.Core;
using GovEventer.ViewModels;

namespace GovEventer.Controllers
{
    public class UserController : AdvancedController
    {
        public ActionResult Login(string returnUrl = "")
        {
            return View(new LoginModel() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = Db.Users.FirstOrDefault(x => x.EMailAddress.Equals(loginModel.EMailAddress, StringComparison.OrdinalIgnoreCase));
                if (user != null && loginModel.Password.VerifyHash(user.PasswordHash))
                {
                    DataUser = user;
                    FormsAuthentication.SetAuthCookie(user.EMailAddress, loginModel.RememberMe);
                    return
                        Redirect(string.IsNullOrEmpty(loginModel.ReturnUrl)
                            ? Url.Action("Index", "General")
                            : loginModel.ReturnUrl);
                }
                ModelState.AddModelError("", "Kullanıcı bulunamadı yada şifre yanlış");
            }
            return View(loginModel);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "General");
        }
    }
}