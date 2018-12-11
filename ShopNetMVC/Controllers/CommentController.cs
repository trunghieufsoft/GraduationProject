using AutoMapper;
using Models.Common;
using Models.DataAccess;
using Models.DataAccess.Dto;
using Models.EntityFramework;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ShopNetMVC.Controllers
{
    public class CommentController : Controller
    {
        public JsonResult AddComment(string comment, int productId)
        {
            var user = (UserSession)Session[Constants.USER_SESSION];
            if (user == null)
            {
                return Json(new { status = false, message = "Vui lòng đăng nhập" }, JsonRequestBehavior.AllowGet);
            }

            var model = new Comment
            {
                Content = comment,
                UserID = user.UserName,
                ProdId = productId
            };

            CommentDao.Instance.insert(model);

            return Json(new { model, status = true }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetComments(int productId)
        {
            var comments = CommentDao.Instance.GetComments(productId);

            var model = Mapper.Map<List<CommentRequestDto>>(comments);

            return Json(new { model, count = model.Count }, JsonRequestBehavior.AllowGet);
        }
    }
}