using AutoMapper;
using Models.Common;
using Models.DataAccess;
using Models.DataAccess.Dto;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ShopNetMVC.Controllers
{
    public class RatingController : Controller
    {
        [HttpPost]
        public ActionResult Index()
        {
            return View();
        }

        // POST: add rating
        [HttpPost]
        public JsonResult AddRating(string content, double rating, int product)
        {
            var prd = ProductDao.Instance.getByID(product);
            var user = (UserSession)Session[Constants.USER_SESSION];
            if (user == null)
            {
                return Json(new { status = false, message = "Vui lòng đăng nhập" });
            }

            var model = new Rating
            {
                Content = content,
                UserID = user.UserID,
                ProdID = product,
                CreatedAt = DateTime.Now,
                Level = rating
            };

            RatingDao.Instance.insert(model);

            return Json(new { status = true, prdName = prd.ProdName });
        }

        public ActionResult EvaluationChart(int code)
        {
            Tuple<List<double>, List<double>, double> data = GetMediumStart(code);
            ViewBag.CountRating = data.Item1;
            ViewBag.PercentStar = data.Item2;
            ViewBag.MediumStar = data.Item3;
            return PartialView("_EvaluationChart");
        }

        private Tuple<List<double>, List<double>, double> GetMediumStart(int code)
        {
            var product = ProductDao.Instance.getByID(code);
            // rating star
            var LsRating = new List<RatingRequestDto>();
            var ListRat = RatingDao.Instance.getObjectList();
            LsRating = Mapper.Map<List<RatingRequestDto>>(ListRat);
            var CountStar = new List<double>() { 0, 0, 0, 0, 0 };
            var PercentStar = new List<double>();
            foreach (var item in LsRating)
            {
                if (item.ProdID == product.ProdID)
                {
                    switch (item.Level.ToString())
                    {
                        case "0":
                        case "0.5":
                        case "1":
                            CountStar[0] += 1;
                            break;
                        case "1.5":
                        case "2":
                            CountStar[1] += 1;
                            break;
                        case "2.5":
                        case "3":
                            CountStar[2] += 1;
                            break;
                        case "3.5":
                        case "4":
                            CountStar[3] += 1;
                            break;
                        case "4.5":
                        default:
                            CountStar[4] += 1;
                            break;
                    }
                }
            }
            foreach (var item in CountStar)
            {
                PercentStar.Add(Math.Round((item / LsRating.Count) * 100, 2));
            }
            var medium = 5 * CountStar[4] + 4 * CountStar[3] + 3 * CountStar[2] + 2 * CountStar[1] + CountStar[0];
            medium = medium / (CountStar[0] + CountStar[1] + CountStar[2] + CountStar[3] + CountStar[4]);
            var medi = medium - (int)medium;
            medium = medi > 0.76 ? (int)medium + 1 : medi > 0.33 ? (int)medium + 0.5 : (int)medium;
            return Tuple.Create(CountStar, PercentStar, medium);
        }

        public JsonResult getFeedback(int product, int page, int pageSize)
        {
            int totalPages, totalRows;
            var ratings = RatingDao.Instance.GetRatings(product, page, pageSize, out totalPages, out totalRows);
            var Uname = new List<string>();
            foreach (var rat in ratings)
            {
                Uname.Add(UserDao.Instance.getByID(rat.UserID).FullName);
            }
            
            return Json(new { data = Mapper.Map<List<RatingRequestDto>>(ratings), Uname, totalPages, totalRows }, JsonRequestBehavior.AllowGet);
        }
    }
}