using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Globalization;
using System.Linq;
using System.Threading;
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
            int doluKoltukSize = lsBilet.Count;
            for (int i = 1; i < 37; i++)
            {
                for (int j = 0; j < doluKoltukSize; j++)
                {
                    if (lsBilet[j].KoltukNo == i)
                        break;

                    if (j == doluKoltukSize - 1)
                    {
                        Bilet bosBilet = new Bilet();
                        bosBilet.KoltukNo = i;
                        bosBilet.SeferID = id;
                        lsBilet.Add(bosBilet);
                        break;
                    }
                }
            }
            lsBilet = lsBilet.OrderBy(o => o.KoltukNo).ToList();
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

        //[Route("/Home/GetSeferVeBilet/{id}")]
        public JsonResult GetSeferBilgileri(int id)
        {
            var query =
                db.Sefer.Join(db.Bilet, sefer => sefer.SeferID, bilet => bilet.SeferID, (sefer, bilet) => sefer)
                    .Where(s => s.SeferID == id).FirstOrDefault();
          //    List<Sefer> lsSefer = query;
         //   lsSefer = lsSefer.ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult BosBiletAl(FormCollection form)
        {
            if (Session["id"] == null)
            {
                ViewBag.mesaj = "Lütfen Giriş Yapınız.";
                return View();
            }
            else
            {
                ValeDBEntities db = new ValeDBEntities();
                Bilet model = new Bilet();
                string str = form["adSoyad"];
                string[] adSoyad = str.Split();
                string ad = adSoyad[0];
                string soyad = adSoyad[1];
                model.MusteriAd = ad;
                model.MusteriSoyad = soyad;
                model.KoltukNo = Convert.ToInt32(form["koltukNo"]);
                model.MusteriID = Convert.ToInt32(Session["id"]);
                model.MusteriCinsiyet = form["cinsiyet"];
                model.Ucret = Convert.ToDouble(form["ucret"]);
                model.IslemTipi = "Kredi Kartı";
                model.BiletTipi = "İnternet";
                string tarih = DateTime.Now.ToString("d"); //mm/dd/yyyy
                string yil = tarih.Substring(6, 4);
                string ay = tarih.Substring(0, 2);
                string gun = tarih.Substring(3, 2);
                string sqlTarih = gun + "/" + ay + "/" + yil;
                model.IslemZamani = sqlTarih;

                string kalkisTarihi = form["kalkisTarihi"];
                TimeSpan kalkisSaati = TimeSpan.Parse(form["kalkisSaati"]);
                int seferId = db.Sefer
                    .Where(s => s.KalkisTarihi==kalkisTarihi && s.KalkisSaati==kalkisSaati && s.KalkisSehri==form["kalkisSehri"] && s.VarisSehri==form["varisSehri"])
                    .Select(s => s.SeferID)
                    .FirstOrDefault();
                model.SeferID = seferId;
                // model.DogumTarihi = form["dogumTarihi"];
                db.Bilet.Add(model);
                db.SaveChanges();
                return View("Index");
            }
          
        }

    }
}