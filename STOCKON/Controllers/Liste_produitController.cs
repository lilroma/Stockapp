using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STOCKON.Models;
using Telerik.Web.Mvc;

namespace STOCKON.Controllers
{ 
    public class Liste_produitController : Controller
    {
        private STOCKONEntities db = new STOCKONEntities();

        //
        // GET: /Liste_produit/

        public ViewResult Index(short id)
        {
            //short id = 1;

            var f = from fr in db.Liste_produit
                    where fr.Id_operation == id
                    select fr;
                   /* select new ProduitL
                    {
                        Id_article = fr.Id_article,
                        Id_operation = fr.Id_operation,
                        Libelle_article = fr.Article.Libelle_article,
                        Conditionnement = fr.Article.Conditionnement,
                        Quantite = fr.Quantite,
                        Prix = fr.Prix,
                        Article = new Arti
                        {
                            Id_article = fr.Article.Id_article,
                            Libelle_article = fr.Article.Libelle_article,
                        }

                    };*/

            // ViewData["Liste_Produit"] = f.ToList(); 

            //  ViewData["Liste_Produit"] = db.Liste_produit.ToList(); 
           // HttpContext.Application.Add("pr", id);
            ViewData["op"] = db.Mouvement_stock.Find(id);
         /*   var art = from ar in db.Article
                      select new Arti
                      {
                          Id_article = ar.Id_article,
                          Libelle_article = ar.Libelle_article,
                      };*/

            ViewData["Articles"] = db.Article.ToList();

            return View(f.ToList());
        }

        [GridAction]
        public ActionResult AjaxBinding(/*short id, string datem, string libelle, string type*/)
        {

            short id = short.Parse(HttpContext.Application["pr"].ToString());

            var f = from fr in db.Liste_produit
                    where fr.Id_operation == id
                    select new ProduitL
                    {
                        Id_article = fr.Id_article,
                        Id_operation = fr.Id_operation,
                        Libelle_article = fr.Article.Libelle_article,
                        Conditionnement = fr.Article.Conditionnement,
                        Quantite = fr.Quantite,
                        Prix = fr.Prix,
                        Article = new Arti
                        {
                            Id_article = fr.Article.Id_article,
                            Libelle_article = fr.Article.Libelle_article,
                        }

                    };

            // ViewData["Liste_Produit"] = f.ToList(); 

            //  ViewData["Liste_Produit"] = db.Liste_produit.ToList(); 
            ViewData["op"] = db.Mouvement_stock.Find(id);
            var art = from ar in db.Article
                      select new Arti
                      {
                          Id_article = ar.Id_article,
                          Libelle_article = ar.Libelle_article,
                      };

            ViewData["Article"] = art.ToList();

            return View(f.ToList());
        }



        [HttpPost]
        [GridAction]
        public ActionResult Insert(short Article)
        {

            short id = short.Parse(HttpContext.Application["pr"].ToString());

            ProduitL produitl = new ProduitL();

            if (TryUpdateModel(produitl, null, null, new[] { "Article" }))
            {
                Liste_produit liste_produit = new Liste_produit
                {

                    Id_operation = id,
                    Id_article = Article,
                    Prix = produitl.Prix,
                    Quantite = produitl.Quantite

                };
                db.Liste_produit.Add(liste_produit);
                db.SaveChanges();

            }

            var art = from ar in db.Article
                      select new Arti
                      {
                          Id_article = ar.Id_article,
                          Libelle_article = ar.Libelle_article,
                      };

            ViewData["Article"] = art.ToList();
            return View();
        }

        //
        // GET: /Liste_produit/Details/5

        public ViewResult Details(short id)
        {
            Liste_produit liste_produit = db.Liste_produit.Find(id);
            return View(liste_produit);
        }

        //
        // GET: /Liste_produit/Create

        public ActionResult Create()
        {
            ViewBag.Id_article = new SelectList(db.Article, "Id_article", "Libelle_article");
            ViewBag.Id_operation = new SelectList(db.Mouvement_stock, "ID_mouvement", "Date_mouvement");
            return View();
        } 

        //
        // POST: /Liste_produit/Create

        [HttpPost]
        public ActionResult Create(Liste_produit liste_produit)
        {
            if (ModelState.IsValid)
            {
                db.Liste_produit.Add(liste_produit);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.Id_article = new SelectList(db.Article, "Id_article", "Libelle_article", liste_produit.Id_article);
            ViewBag.Id_operation = new SelectList(db.Mouvement_stock, "ID_mouvement", "Date_mouvement", liste_produit.Id_operation);
            return View(liste_produit);
        }
        
        //
        // GET: /Liste_produit/Edit/5
 
        public ActionResult Edit(short id)
        {
            Liste_produit liste_produit = db.Liste_produit.Find(id);
            ViewBag.Id_article = new SelectList(db.Article, "Id_article", "Libelle_article", liste_produit.Id_article);
            ViewBag.Id_operation = new SelectList(db.Mouvement_stock, "ID_mouvement", "Date_mouvement", liste_produit.Id_operation);
            return View(liste_produit);
        }

        //
        // POST: /Liste_produit/Edit/5

        [HttpPost]
        public ActionResult Edit(Liste_produit liste_produit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(liste_produit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_article = new SelectList(db.Article, "Id_article", "Libelle_article", liste_produit.Id_article);
            ViewBag.Id_operation = new SelectList(db.Mouvement_stock, "ID_mouvement", "Date_mouvement", liste_produit.Id_operation);
            return View(liste_produit);
        }

        //
        // GET: /Liste_produit/Delete/5
 
        public ActionResult Delete(short id)
        {
            Liste_produit liste_produit = db.Liste_produit.Find(id);
            return View(liste_produit);
        }

        //
        // POST: /Liste_produit/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {            
            Liste_produit liste_produit = db.Liste_produit.Find(id);
            db.Liste_produit.Remove(liste_produit);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}