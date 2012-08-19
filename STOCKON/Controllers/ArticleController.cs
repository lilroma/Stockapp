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
    public class ArticleController : Controller
    {
        private STOCKONEntities db = new STOCKONEntities();

        //
        // GET: /Article/

        public ViewResult Index(string searchString)
        {
            var articles = from ar in db.Article.Include(a => a.Famille_article)
                               select ar;
            
            
            if (!String.IsNullOrEmpty(searchString))
            {
                articles = articles.Where(ar => ar.Libelle_article.ToUpper().Contains(searchString.ToUpper())
                                       || ar.Famille_article.Libelle_famille.ToUpper().Contains(searchString.ToUpper()));
            }
          
            
            return View(articles.ToList());
        }

        public ViewResult Lvente(short id)
        {
            var liste_produit_ventes = from l in db.Liste_produit_vente.Include(a => a.Article).Include(a => a.Facture_vente).Include(a => a.Facture_vente)
                                       where l.Id_article == id
                                       select l;

            return View(liste_produit_ventes.ToList());

        }

        public ViewResult Lachat(short id)
        {
            var liste_produit_achat = from l in db.Liste_produit_achat.Include(a => a.Article).Include(a => a.Facture_achat).Include(a => a.Facture_achat.Fournisseur)
                                       where l.Id_article == id
                                       select l;

            return View(liste_produit_achat.ToList());

        }

        public ViewResult Lmv(short id)
        {
            var liste_produit = from l in db.Liste_produit.Include(a => a.Article).Include(a => a.Mouvement_stock)
                                      where l.Id_article == id
                                      select l;

            return View(liste_produit.ToList());

        }

        //
        // GET: /Article/Details/5

        public ViewResult Details(short id)
        {
            Article article = db.Article.Find(id);
            return View(article);
        }

        //
        // GET: /Article/Create

        public ActionResult Create()
        {
           // ViewBag.Id_famille = new SelectList(db.Famille_article, "Id_famille", "Libelle_famille");

            var familles = db.Famille_article.ToList();
            Article article = new Article();

            var art = db.Article.Max(a => a.Id_article) + 1;

            article.Code_article = MyGlobalVariables.FormatCode("ART", art);

            List<String> listToBind = new List<String>();

            foreach (var item in familles)
            {
                listToBind.Add(item.Libelle_famille + " - " + item.Id_famille);
            }

            ViewData["Famille"] = listToBind;
            
            return View(article);
        } 

        //
        // POST: /Article/Create

        [HttpPost]
        public ActionResult Create(Article article, string test)
        {

            var ar = from a in db.Article
                     where a.Code_article == article.Code_article
                     select a;

            if (ModelState.IsValid)
            {
              try
               {
                    if (ar.ToList().Count>0)
                    {
                        ModelState.AddModelError("", "Le code que vous avez specifié est déjà utilisé");
                    }
                    else
                    {
                        article.Qte_article = 0;
                        int px = test.IndexOf("-") + 2;
                        int p = test.Length - px;
                        string v = test.Substring(px, p);
                        article.Id_famille = short.Parse(v);
                        db.Article.Add(article);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                   }
               }
                catch
                {
                    ModelState.AddModelError("", "La famille de l'article est incorrecte");
                } 
            }

            //ViewBag.Id_famille = new SelectList(db.Famille_article, "Id_famille", "Libelle_famille", article.Id_famille);
           // ViewData["Famille"] = new SelectList(db.Famille_article, "Id_famille", "Libelle_famille", article.Id_famille);
            var familles = db.Famille_article.ToList();

            List<String> listToBind = new List<String>();

            foreach (var item in familles)
            {
                listToBind.Add(item.Libelle_famille + " - " + item.Id_famille);
            }

            ViewData["Famille"] = listToBind;
            return View(article);
        }
        
        //
        // GET: /Article/Edit/5
 
        public ActionResult Edit(short id)
        {
            Article article = db.Article.Find(id);
            var familles = db.Famille_article.ToList();

            List<String> listToBind = new List<String>();

            foreach (var item in familles)
            {
                listToBind.Add(item.Libelle_famille + " - " + item.Id_famille);
            }

            ViewData["Famille"] = listToBind;
            //ViewBag.Id_famille = new SelectList(db.Famille_article, "Id_famille", "Libelle_famille", article.Id_famille);
            return View(article);
        }

        //
        // POST: /Article/Edit/5

        [HttpPost]
        public ActionResult Edit(Article article, string test)
        {
            if (ModelState.IsValid)
            {
                //Article articles = new Article();
                    
                  //articles  = db.Article.Find(article.Id_article);
                 //article.Qte_article = articles.Qte_article;
                try
                {
                    article.Qte_article = 0;
                    int px = test.IndexOf("-") + 2;
                    int p = test.Length - px;
                    string v = test.Substring(px, p);
                    article.Id_famille = short.Parse(v);

                    db.Entry(article).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "La famille de l'article est incorrecte");
                }
            }
            var familles = db.Famille_article.ToList();

            List<String> listToBind = new List<String>();

            foreach (var item in familles)
            {
                listToBind.Add(item.Libelle_famille + " - " + item.Id_famille);
            }

            ViewData["Famille"] = listToBind;
           // ViewBag.Id_famille = new SelectList(db.Famille_article, "Id_famille", "Libelle_famille", article.Id_famille);
            return View(article);
        }

        //
        // GET: /Article/Delete/5
 
        public ActionResult Delete(short id)
        {
            Article article = db.Article.Find(id);
            return View(article);
        }

        //
        // POST: /Article/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {
            try
            {
                Article article = db.Article.Find(id);
                db.Article.Remove(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch 
            {
                ModelState.AddModelError("", "Suppression impossible");
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