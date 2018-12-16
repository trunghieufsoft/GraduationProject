using Models.Common;
using Models.DataAccess;
using Models.DataAccess.Dto;
using System.Web.Mvc;

namespace ShopNetMVC.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserDto user)
        {
            if (ModelState.IsValid)
            {
                var result = UserDao.Instance.checkLogin(user);
                switch (result)
                {
                    case Constants.LoginState.PasswordNull:
                        ModelState.AddModelError("", "Vui lòng nhập mật khẩu!");
                        break;

                    case Constants.LoginState.UsernameNull:
                        ModelState.AddModelError("", "Vui lòng nhập tài khoản!");
                        break;

                    case Constants.LoginState.UsernameAndPasswordNull:
                        ModelState.AddModelError("", "Tên hoặc mật khẩu không đúng!");
                        break;

                    case Constants.LoginState.Locked:
                        ModelState.AddModelError("", "Tài khoản đã bị khóa. Vui lòng liên hệ admin để được hỗ trợ!");
                        break;

                    default:
                        var u = UserDao.Instance.getByID(user.UserName);
                        var userSession = new UserSession(u.UserID, u.FullName, u.GrantID);
                        Session.Add(Constants.USER_SESSION, userSession);
                        return RedirectToAction("Index", "Home");
                }
            }
            return View(user);
        }
    }
}