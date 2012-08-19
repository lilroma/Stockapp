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
    public class Liste_produit_achatController : Controller
    {
        //
        // GET: /Liste_produit_achat/
        private STOCKONEntities db = new STOCKONEntities();

        public ActionResult Index(short idm = 0)
        {
            if (idm == 0)
                idm = STOCKON.MyGlobalVariables.faid;
            else
                STOCKON.MyGlobalVariables.faid = idm;

            var article = db.Article;
            ViewData["articles"] = article.ToList();

            List<String> listToBind = new List<String>();

            Preference preference = new Preference();
            preference = db.Preference.Find(1);

            if (preference.CRecherche.Trim() == "Code article".Trim())
            {
                foreach (var item in article.ToList())
                {
                    listToBind.Add(item.Code_article + " - " + item.Id_article);
                }
            }
            else
            {
                foreach (var item in article.ToList())
                {
                    listToBind.Add(item.Libelle_article + " - " + item.Id_article);
                }
            }
            ViewData["art"] = listToBind;

            ViewData["Fa"] = db.Facture_achat.Find(idm);

            var liste_produit = from l in db.Liste_produit_achat.Include(l => l.Article).Include(l => l.Facture_achat)
                                where l.Id_facture == idm
                                select l;
            return View(liste_produit.ToList());
        }

        public ViewResult Insert(short idm)
        {
            //  short id = 1;
            STOCKON.MyGlobalVariables.faid = idm;

            var article = db.Article;
            ViewData["articles"] = article.ToList();

            List<String> listToBind = new List<String>();

            Preference preference = new Preference();
            preference = db.Preference.Find(1);

            if (preference.CRecherche.Trim() == "Code article".Trim())
            {
                foreach (var item in article.ToList())
                {
                    listToBind.Add(item.Code_article + " - " + item.Id_article);
                }
            }
            else
            {
                foreach (var item in article.ToList())
                {
                    listToBind.Add(item.Libelle_article + " - " + item.Id_article);
                }
            }
            ViewData["art"] = listToBind;


            ViewData["Fa"] = db.Facture_achat.Find(idm);

            var liste_produit = from l in db.Liste_produit_achat.Include(l => l.Article).Include(l => l.Facture_achat)
                                where l.Id_facture == idm
                                select l;
            return View(liste_produit.ToList());
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Insert(short idm, string article)
        {
            //Create a new instance of the Customer class.
            try
            {
                int px = article.IndexOf("-") + 2;
                int p = article.Length - px;
                string v = article.Substring(px, p);
                
                Liste_produit_achat customer = new Liste_produit_achat
                {
                    Id_article = short.Parse(v),
                    Id_facture = idm
                };

                Facture_achat Fa = new Facture_achat();
                Fa = db.Facture_achat.Find(idm);

                Article Art = new Article();
                Art = db.Article.Find(customer.Id_article);

                //Perform model binding (fill the customer properties and validate it).
                if (TryUpdateModel(customer))
                {
                    //The model is valid - insert the customer and redisplay the grid.
                    customer.Prix_total = customer.Prix * customer.Quantite;
                    db.Liste_produit_achat.Add(customer);
                    db.SaveChanges();

                    if (Fa.Type_facture.Trim() == "Bon de commande".Trim())
                    { }
                    else
                    {
                        Art.Qte_article = Art.Qte_article + customer.Quantite;
                        db.Entry(Art).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    //GridRouteValues() is an extension method which returns the
                    //route values defining the grid state - current page, sort expression, filter etc.
                    return RedirectToAction("Index", new { idm = customer.Id_facture });
                }

                if (customer.Quantite > 0)
                {
                    customer.Prix_total = customer.Prix * customer.Quantite;
                    db.Liste_produit_achat.Add(customer);
                    db.SaveChanges();
                    if (Fa.Type_facture.Trim() == "Bon de commande".Trim())
                    { }
                    else
                    {
                        Art.Qte_article = Art.Qte_article + customer.Quantite;
                        db.Entry(Art).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index", new { idm = customer.Id_facture });
                }
                //The model is invalid - render the current view to show any validation errors.
            }
            catch
            {
                ModelState.AddModelError("", "l'article est incorrect");
            }
            var articles = db.Article;
            ViewData["articles"] = articles.ToList();

            List<String> listToBind = new List<String>();

            Preference preference = new Preference();
            preference = db.Preference.Find(1);

            if (preference.CRecherche.Trim() == "Code article".Trim())
            {
                foreach (var item in articles.ToList())
                {
                    listToBind.Add(item.Code_article + " - " + item.Id_article);
                }
            }
            else
            {
                foreach (var item in articles.ToList())
                {
                    listToBind.Add(item.Libelle_article + " - " + item.Id_article);
                }
            }
            ViewData["art"] = listToBind;


            ViewData["Fa"] = db.Facture_achat.Find(idm);

            var liste_produit = from l in db.Liste_produit_achat.Include(l => l.Article).Include(l => l.Facture_achat)
                                where l.Id_facture == idm
                                select l;
            return View("index",liste_produit.ToList());

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteP(short id, short idm)
        {

            var liste_produit1 = from l in db.Liste_produit_achat.Include(l => l.Article).Include(l => l.Facture_achat)
                                 where l.Id_facture == idm && l.Id_article == id
                                 select l;

            Liste_produit_achat L = liste_produit1.First();

            Facture_achat Fa = new Facture_achat();
            Fa = db.Facture_achat.Find(idm);

            Article article = db.Article.Find(id);
            if (Fa.Type_facture.Trim() == "Bon de commande".Trim())
            { }
            else
            {
                article.Qte_article = article.Qte_article - L.Quantite;
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
            }
            db.Liste_produit_achat.Remove(L);
            db.SaveChanges();

            return RedirectToAction("Index", new { idm = idm });

            /* var articles = db.Article;
             ViewData["articles"] = articles.ToList();

             ViewData["op"] = db.Mouvement_stock.Find(idm);

             var liste_produit = from l in db.Liste_produit.Include(l => l.Article).Include(l => l.Mouvement_stock)
                                 where l.Id_operation == idm
                                 select l;
             return View(liste_produit.ToList());*/
        }

        [HttpPost]
        public ActionResult Update(short id, short idm)
        {

            //Create a new instance of the Customer class.
            var liste_produit1 = from l in db.Liste_produit_achat.Include(l => l.Article).Include(l => l.Facture_achat)
                                 where l.Id_facture == idm && l.Id_article == id
                                 select l;

            Facture_achat Fa = new Facture_achat();
            Fa = db.Facture_achat.Find(idm);

            Liste_produit_achat L = liste_produit1.First();
            var QteP = L.Quantite;

            //Article Art = new Article();
           // Art = db.Article.Find(L.Id_article);

            //Perform model binding (fill the customer properties and validate it).
            //L.Id_article = article;
            if (TryUpdateModel(L))
            {
                //The model is valid - insert the customer and redisplay the grid.
                L.Prix_total = L.Prix * L.Quantite;
                db.Entry(L).State = EntityState.Modified;
                db.SaveChanges();

                if (Fa.Type_facture.Trim() == "Bon de commande".Trim())
                { }
                else
                {
                    L.Article.Qte_article = L.Article.Qte_article - QteP + L.Quantite;
                    db.Entry(L.Article).State = EntityState.Modified;
                    db.SaveChanges();
                }
               /* Art.Qte_article = Art.Qte_article - QteP;
                db.Entry(Art).State = EntityState.Modified;
                db.SaveChanges();*/

                //GridRouteValues() is an extension method which returns the
                //route values defining the grid state - current page, sort expression, filter etc.
                return RedirectToAction("Index", new { idm = L.Id_facture });
            }

            if (L.Quantite > 0)
            {
                L.Prix_total = L.Prix * L.Quantite;
                db.Entry(L).State = EntityState.Modified;
                db.SaveChanges();
                if (Fa.Type_facture.Trim() == "Bon de commande".Trim())
                { }
                else
                {
                    L.Article.Qte_article = L.Article.Qte_article - QteP + L.Quantite;
                    db.Entry(L.Article).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index", new { idm = L.Id_facture });

             /*   Art.Qte_article = Art.Qte_article - QteP;
                db.Entry(Art).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { idm = L.Id_facture });*/
            }
            //The model is invalid - render the current view to show any validation errors.
            var articles = db.Article;
            ViewData["articles"] = articles.ToList();

            ViewData["Fa"] = db.Facture_achat.Find(idm);

            var liste_produit = from l in db.Liste_produit_achat.Include(l => l.Article).Include(l => l.Facture_achat)
                                where l.Id_facture == idm
                                select l;
            return View("Index", liste_produit.ToList());
        }

        //
        // GET: /Liste_produit_achat/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Liste_produit_achat/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Liste_produit_achat/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Liste_produit_achat/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Liste_produit_achat/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Liste_produit_achat/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Liste_produit_achat/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
