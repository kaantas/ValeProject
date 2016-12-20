using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ValeProject.Models;

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
        //Üye Girişi Sayfasındaki Form Post Edildiğinde (Authentication)
        [HttpPost]
        public ActionResult UyeGirisi(FormCollection form)
        {
            string email = form["email"];
            string sifre = form["sifre"];
            var context = new ValeDBEntities();
            var query = context.Musteri.ToList();
            var result = query.Where(m => m.Email == email && m.Sifre == sifre).ToList();
            if (result.Count > 0) 
                ViewBag.mesaj = result[0].Ad;
            else 
                ViewBag.mesaj = "Kullanıcı Bulunamadı !";
            
            return View();
        }
        //Üye Ol
        public ActionResult UyeOl()
        {
            return View();
        }
        //Üye Ol Ekranındaki Form Post Edilince
        [HttpPost]
        public ActionResult UyeOl(FormCollection form)
        {
            ValeDBEntities db = new ValeDBEntities();
            Musteri model = new Musteri();
            model.Ad = form["ad"];
            model.Soyad = form["soyad"];
            model.Email = form["email"];
            model.Cinsiyet = form["cinsiyet"];
           // model.DogumTarihi = form["dogumTarihi"];
            model.Adres = form["adres"];
            model.Tel = form["telefon"];
            model.Sifre = form["sifre"];

            db.Musteri.Add(model);
            db.SaveChanges();
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
        public ActionResult BiletAl()
        {
            return View();   
        }

        public JsonResult GetBilet()
        {
            /*
            List<Bilet> lsBilet = null;
            var query = db.Bilet.ToList();
            var result = query.Where(b => b.SeferID == 1).ToList();
            if (result.Count > 0)
            {
                lsBilet = result;
            }
            return Json(lsBilet,JsonRequestBehavior.AllowGet);*/
            /*
            var query =
                db.Bilet.Join(db.Sefer, bilet => bilet.SeferID, sefer => sefer.SeferID, (bilet, sefer) => bilet)
                    .Where(b => b.SeferID == 1)
                    .ToList();
            List<Bilet> lsBilet = query;
            */



            List<Bilet> lsBilet = db.Bilet.ToList();
           var json = new
            {
                MusteriAd = "Kaan",
                MusteriSoyad = "Taş",
                MusteriCinsiyet = "Erkek",
                KoltukNo = 1
            };
       //     var json = Json(lsBilet, JsonRequestBehavior.AllowGet);
            return Json(json, JsonRequestBehavior.AllowGet);
            
        }
        [HttpPost]
        public ActionResult BiletAl(FormCollection form)
        {
            Sefer model = new Sefer();
            List<Sefer> ls=null;
            string kalkisSehri = form["kalkisSehri"];
            string varisSehri = form["varisSehri"];

            string tarih = form["tarih"]; // mm/dd/yyyy
            string yil = tarih.Substring(6,4);
            string ay = tarih.Substring(0, 2);
            string gun = tarih.Substring(3, 2);
            string sqlTarih = gun + "/" + ay + "/" + yil;
            
            var context = new ValeDBEntities();
            var query = context.Sefer.ToList();
            var result = query.Where(m => m.KalkisSehri == kalkisSehri && m.VarisSehri == varisSehri && m.KalkisTarihi == sqlTarih).OrderBy(m => m.KalkisSaati).ToList();
            if (result.Count > 0)
            {
                ls = result;
                return View(ls);
            }
            else
            {
                ViewBag.mesaj = "Sefer Bulunamadı !";
                return View();
            }
        }
    }
}