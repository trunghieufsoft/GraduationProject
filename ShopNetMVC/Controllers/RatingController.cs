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
        // GET: Rating
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
            ViewBag.CountRating = CountStar;
            foreach (var item in CountStar)
            {
                PercentStar.Add(Math.Round((item / LsRating.Count) * 100, 2));
            }
            ViewBag.PercentStar = PercentStar;
            var medium = Math.Round((PercentStar[0] + PercentStar[1] + PercentStar[2] + PercentStar[3] + PercentStar[4]) / 500, 2);
            double medi = (medium - (int)medium) * -1;
            ViewBag.MediumStar = medi > 0.5 ? (int)medium + 1 : medium < 0.24 ? (int)medium : (int)medium + 0.5;
            return PartialView("_EvaluationChart");
        }
    }
}