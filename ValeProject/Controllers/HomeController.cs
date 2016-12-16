﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
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
    }
}