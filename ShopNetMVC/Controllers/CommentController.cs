using AutoMapper;
using Models.Common;
using Models.DataAccess;
using Models.DataAccess.Dao;
using Models.DataAccess.Dto;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ShopNetMVC.Controllers
{
    public class CommentController : Controller
    {
        public JsonResult AddComment(string comment, int prodId)
        {
            var user = (UserSession)Session[Constants.USER_SESSION];
            if (user == null)
            {
                return Json(new { status = false, message = "Vui lòng đăng nhập" }, JsonRequestBehavior.AllowGet);
            }

            var model = new Comment
            {
                Content = comment,
                UserID = user.UserID,
                ProdId = prodId,
                CreatedAt = DateTime.Now
            };

            CommentDao.Instance.insert(model);

            return Json(new { model, status = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddReply(string comment, string comId)
        {
            var user = (UserSession)Session[Constants.USER_SESSION];
            if (user == null)
            {
                return Json(new { status = false, message = "Vui lòng đăng nhập" }, JsonRequestBehavior.AllowGet);
            }
            var reply = new Reply()
            {
                ComID = comId,
                UserID = user.UserID,
                Content = comment,
                CreatedAt = DateTime.Now
            };
            RepliesDao.Instance.insert(reply);
            return Json(new { reply, status = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetComments(int request)
        {
            var user = (UserSession)Session[Constants.USER_SESSION];
            // response.comment: list < object > = [ { comment: Comments, userName: string, replies: Replies[]} ]
            // trong đó: Replies: null | object = { reply: Replies, userName: string}
            var data = CommentDao.Instance.GetComments(request);

            var response = new List<object>();
            foreach (var elm in data)
            {
                var userCom = UserDao.Instance.getByID(elm.UserID);

                var replies = new List<object>();
                var rep = RepliesDao.Instance.getObjectList(elm.ComID, false);
                foreach (var item in rep)
                {
                    var userRep = UserDao.Instance.getByID(item.UserID);
                    replies.Add(new
                    {
                        reply = Mapper.Map<RepliesRequestDto>(item),
                        userName = userRep != default(User) ? userRep.FullName : "",
                        userID = userRep != default(User) ? userRep.UserID : "",
                        userGrant = userRep != default(User) ? userRep.GrantID : -1,
                    });
                }

                response.Add(new
                {
                    comment = Mapper.Map<CommentRequestDto>(elm),
                    userName = userCom != default(User) ? userCom.FullName : "",
                    userID = user != null ? user.UserID : "",
                    userGrant = userCom != default(User) ? userCom.GrantID : -1,
                    replies = replies
                });
            }
            return Json(new { comment = response, status = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult deleteComment(string request)
        {
            var result = CommentDao.Instance.delete(request);
            return Json(new { result = result });
        }

        [HttpPost]
        public JsonResult deleteReply(string comId, int repNo)
        {
            var result = RepliesDao.Instance.delete(repNo, comId);
            return Json(new { result = result });
        }
    }
}