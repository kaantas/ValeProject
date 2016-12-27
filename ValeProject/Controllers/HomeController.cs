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
            return View();
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
            {
                Session["adsoyad"] = result[0].Ad + " " + result[0].Soyad;
                Session["id"] = result[0].MusteriID;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.mesaj = "Kullanıcı Bulunamadı !";
                return View();
            }
                
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
        public ActionResult PersonelGirisi()
        {
            return View();
        }
        //Yönetici Girişi sayfasına post yapıldıgında
        [HttpPost]
        public ActionResult PersonelGirisi(FormCollection form)
        {
            string email = form["email"];
            string sifre = form["password"];
            var context = new ValeDBEntities(); 
            var query = context.Personel.ToList();
            int personelId = query.Where(m => m.Email == email)
                .Select(m=>m.PersonelID)
                .FirstOrDefault();
            var adminQuery = context.Admin.ToList();
            var result = adminQuery.Where(a => a.PersonelID == personelId && a.Sifre == sifre).ToList();
            if (result.Count > 0)
            {
                Session["adsoyad"] = result[0].Personel.PersonelAd + " " + result[0].Personel.PersonelSoyad;
                Session["id"] = result[0].Personel.PersonelID;
                Session["admin"] = result[0].Sifre;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.mesaj = "Kullanıcı Bulunamadı !";
                return View();
            }
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
        [HttpPost]
        public ActionResult BiletAl(FormCollection form)
        {
            Sefer model = new Sefer();
            List<Sefer> ls = null;
            string kalkisSehri = form["kalkisSehri"];
            string varisSehri = form["varisSehri"];
            string yil;
            string ay;
            string gun;
            string tarih = form["tarih"]; // mm/dd/yyyy
            if (tarih == "")
            {
                tarih = DateTime.Now.ToString("d"); //mm/dd/yyyy
                yil = tarih.Substring(6, 4);
                ay = tarih.Substring(0, 2);
                gun = tarih.Substring(3, 2);
            }
            yil = tarih.Substring(6, 4);
            ay = tarih.Substring(0, 2);
            gun = tarih.Substring(3, 2);
            string sqlTarih = gun + "/" + ay + "/" + yil;

            var context = new ValeDBEntities();
            var result = context.Sefer.Include("Otobus")
                .Where(m => m.KalkisSehri == kalkisSehri && m.VarisSehri == varisSehri && m.KalkisTarihi == sqlTarih).OrderBy(m => m.KalkisSaati).ToList();
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
      //[Route("/Home/GetBilet/{id}")]
        public JsonResult GetBilet(int id)
        {
            var query =
                db.Bilet.Join(db.Sefer, bilet => bilet.SeferID, sefer => sefer.SeferID, (bilet, sefer) => bilet)
                    .Where(sefer => sefer.SeferID == id).ToList();
            List<Bilet> lsBilet = query;
            return Json(lsBilet, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Biletlerim()
        {
            List<Bilet> ls = null;
            var context = new ValeDBEntities();
            int id = Convert.ToInt32(Session["id"]);
            var result = context.Bilet.Include("Sefer").Where(b=>b.MusteriID==id).ToList();
            if (result.Count > 0)
            {
                ls = result;
                return View(ls);
            }
            else
            {
                ViewBag.mesaj = "Kayıtlı Biletiniz Bulunmamaktadır.";
                return View();
            }
        }
        public ActionResult SeferEkle()
        {
            return View();
        }
        public ActionResult PersonelEkle()
        {
            return View();
        }
        //Login sırasında exit butonuna bastıgında logout ol
        public ActionResult Exit()
        {
            Session["adsoyad"] = null;
            Session["id"] = null;
            if (Session["admin"] != null)
            {
                Session["admin"] = null;
            }
            return View("Index");
        }
       

    }
}