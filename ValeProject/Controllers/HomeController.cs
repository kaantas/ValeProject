using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ValeProject.Controllers
{
    public class HomeController : Controller
    {
        private ValeDBEntities db = new ValeDBEntities();
        // GET: Home
        //MainPage
        public ActionResult Index()
        {
            List <Musteri> ls = db.Musteri.ToList();
            return View(ls);
        }
        //Üye Girişi
        public ActionResult UyeGirisi()
        {
            return View();
        }
        //Üye Ol
        public ActionResult UyeOl()
        {
            return View();
        }
        //Yönetici Girişi
        public ActionResult YoneticiGirisi()
        {
            return View();
        }
        //İletişim
        public ActionResult Iletisim()
        {
            return View();
        }
    }
}