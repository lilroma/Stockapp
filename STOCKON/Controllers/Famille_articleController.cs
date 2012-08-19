using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STOCKON.Models;

namespace STOCKON.Controllers
{ 
    public class Famille_articleController : Controller
    {
        private STOCKONEntities db = new STOCKONEntities();

        //
        // GET: /Famille_article/

        public ViewResult Index(string searchString)
        {
            var fa = from far in db.Famille_article
                          select far;

            if (!String.IsNullOrEmpty(searchString))
            {
                fa = fa.Where(far => far.Libelle_famille.ToUpper().Contains(searchString.ToUpper()));
            }
            return View(fa.ToList());
        }

        //
        // GET: /Famille_article/Details/5

        public ViewResult Details(short id)
        {
            Famille_article famille_article = db.Famille_article.Find(id);
            return View(famille_article);
        }

        //
        // GET: /Famille_article/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Famille_article/Create

        [HttpPost]
        public ActionResult Create(Famille_article famille_article)
        {
            if (ModelState.IsValid)
            {
                db.Famille_article.Add(famille_article);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(famille_article);
        }
        
        //
        // GET: /Famille_article/Edit/5
 
        public ActionResult Edit(short id)
        {
            Famille_article famille_article = db.Famille_article.Find(id);
            return View(famille_article);
        }

        //
        // POST: /Famille_article/Edit/5

        [HttpPost]
        public ActionResult Edit(Famille_article famille_article)
        {
            if (ModelState.IsValid)
            {
                db.Entry(famille_article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(famille_article);
        }

        //
        // GET: /Famille_article/Delete/5
 
        public ActionResult Delete(short id)
        {
            Famille_article famille_article = db.Famille_article.Find(id);
            return View(famille_article);
        }

        //
        // POST: /Famille_article/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {
            try
            {
                Famille_article famille_article = db.Famille_article.Find(id);
                db.Famille_article.Remove(famille_article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception Ex)
            {
               ModelState.AddModelError("","Suppression impossible, famille rattachée aux articles");
               return View();
            
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}