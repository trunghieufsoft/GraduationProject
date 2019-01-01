using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web.Mvc;

namespace ShopNetMVC.Controllers
{
    public class ContactController : BaseController
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult sentMail(string name, string email, string phone, string content)
        {
            string sentTo = email;
            string subject = "[Contact-Shop][CoffeeHome][" + DateTime.Now.ToLongDateString() + "]- " + name.ToUpper() + " " + phone;
            List<string> body = new List<string>
            {
                name + " - " + phone,
                content
            };
            try
            {
                sentMail_To_Shop(sentTo, subject, body);
                //sentMail_To_Custommer(sentTo, subject, body[1]);
                return Json(new { message = "Cảm ơn bạn đã gửi thông tin về chúng tôi.<br/>Chúng tôi sẽ cố gắng hoàn thiện hơn trong thời gian tơi.", status = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, status = false });
            }
        }
        private void sentMail_To_Custommer(string sentTo, string subject, string body)
        {
            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            string strHost = smtpSection.Network.Host;
            int port = smtpSection.Network.Port;
            string strUserName = smtpSection.Network.UserName;
            string strFromPass = smtpSection.Network.Password;
            SmtpClient smtp = new SmtpClient(strHost, port);
            NetworkCredential cert = new NetworkCredential(strUserName, strFromPass);
            smtp.Credentials = cert;
            smtp.EnableSsl = true;
            MailMessage msg = new MailMessage(smtpSection.From, sentTo);
            msg.IsBodyHtml = true;
            msg.Body += "<h1>Trung tâm quản lý đánh giá chất lượng sản phẩm Coffee Home</h1>";
            msg.Body += "<h3> http://ShopCoffee.com.vn </h3>";
            msg.Body += "Bạn vừa gửi thông tin đến hệ thống web Coffee Home.<br/>";
            msg.Body += "<br/>Cảm ơn bạn đã gửi thông tin về chúng tôi.";
            msg.Body += "<hr/><br/>Thông tin:";
            msg.Body += body;
            msg.Body += "<br/>Chúng tôi sẽ cố gắng hoàn thiện hơn, trong thời gian tới!";
            smtp.Send(msg);
        }
        private void sentMail_To_Shop(string sentFrom, string subject, List<string> body)
        {
            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            string strHost = smtpSection.Network.Host;
            int port = smtpSection.Network.Port;
            string strUserName = smtpSection.Network.UserName;
            string strFromPass = smtpSection.Network.Password;
            SmtpClient smtp = new SmtpClient(strHost, port);
            NetworkCredential cert = new NetworkCredential(strUserName, strFromPass);
            smtp.Credentials = cert;
            smtp.EnableSsl = true;
            MailMessage msg = new MailMessage(sentFrom, smtpSection.From);
            msg.IsBodyHtml = true;
            msg.Body += "<h1>" + body[0] + "</h1>";
            msg.Body += "<h3> http://ShopCoffee.com.vn </h3>";
            msg.Body += "<hr/><b>Thông Tin:</b>";
            msg.Body += "<p style='font-size: 13px'>" + body[1] + "</p>";
            msg.Body += "<br/>" + body[0];
            smtp.Send(msg);
        }
    }
}