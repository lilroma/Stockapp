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
    public class Default1Controller : Controller
    {
        private STOCKONEntities db = new STOCKONEntities();

        //
        // GET: /Default1/

        

        public ViewResult Index(short idm = 0)
        {
          //  short id = 1;

            if (idm == 0)
                idm = STOCKON.MyGlobalVariables.mid;
            else
                STOCKON.MyGlobalVariables.mid = idm;

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

            ViewData["op"] = db.Mouvement_stock.Find(idm);

            var liste_produit = from l in db.Liste_produit.Include(l => l.Article).Include(l => l.Mouvement_stock)
                                where l.Id_operation == idm
                                select l;
            return View(liste_produit.ToList());
        }

        public ViewResult Insert(short idm)
        {
            //  short id = 1;
             STOCKON.MyGlobalVariables.mid = idm;
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

            ViewData["op"] = db.Mouvement_stock.Find(idm);

            var liste_produit = from l in db.Liste_produit.Include(l => l.Article).Include(l => l.Mouvement_stock)
                                where l.Id_operation == idm
                                select l;
            return View(liste_produit.ToList());
        }

       
  [AcceptVerbs(HttpVerbs.Post)]        
public ActionResult Insert(short idm, string article)
{

    try
    {
        int px = article.IndexOf("-") + 2;
        int p = article.Length - px;
        string v = article.Substring(px, p);

        //Create a new instance of the Customer class.
        Liste_produit customer = new Liste_produit
        {
            Id_article = short.Parse(v),
            Id_operation = idm
        };

        Article Art = new Article();
        Mouvement_stock Mv = new Mouvement_stock();

        Art = db.Article.Find(customer.Id_article);
        Mv = db.Mouvement_stock.Find(idm);

        //Perform model binding (fill the customer properties and validate it).
        if (TryUpdateModel(customer))
        {
            if (Mv.Type_mouvement == "Sortie de stock")
            {
                if (Art.Qte_article < customer.Quantite)
                {
                    ModelState.AddModelError("", "La quantité est supérieure à la quantité en stock");
                }
                else
                {
                    //The model is valid - insert the customer and redisplay the grid.
                    customer.Prix_Total = customer.Quantite * customer.Prix;
                    db.Liste_produit.Add(customer);
                    db.SaveChanges();
                    //modiification de la qte d'article
                    Art.Qte_article = Art.Qte_article - customer.Quantite;
                    db.Entry(Art).State = EntityState.Modified;
                    db.SaveChanges();
                    //GridRouteValues() is an extension method which returns the
                    //route values defining the grid state - current page, sort expression, filter etc.
                    return RedirectToAction("Index", new { idm = customer.Id_operation });
                }

            }
            else
            {
                //The model is valid - insert the customer and redisplay the grid.
                customer.Prix_Total = customer.Quantite * customer.Prix;
                db.Liste_produit.Add(customer);
                db.SaveChanges();
                //modiification de la qte d'article
                Art.Qte_article = Art.Qte_article + customer.Quantite;
                db.Entry(Art).State = EntityState.Modified;
                db.SaveChanges();
                //GridRouteValues() is an extension method which returns the
                //route values defining the grid state - current page, sort expression, filter etc.
                return RedirectToAction("Index", new { idm = customer.Id_operation });
            }
        }

        if (customer.Quantite > 0)
        {
            if (Mv.Type_mouvement == "Sortie de stock")
            {
                if (Art.Qte_article < customer.Quantite)
                {
                    ModelState.AddModelError("", "La quantité est supérieure à la quantité en stock");
                }
                else
                {
                    //The model is valid - insert the customer and redisplay the grid.
                    customer.Prix_Total = customer.Quantite * customer.Prix;
                    db.Liste_produit.Add(customer);
                    db.SaveChanges();
                    //modiification de la qte d'article
                    Art.Qte_article = Art.Qte_article - customer.Quantite;
                    db.Entry(Art).State = EntityState.Modified;
                    db.SaveChanges();
                    //GridRouteValues() is an extension method which returns the
                    //route values defining the grid state - current page, sort expression, filter etc.
                    return RedirectToAction("Index", new { idm = customer.Id_operation });
                }

            }
            else
            {
                //The model is valid - insert the customer and redisplay the grid.
                customer.Prix_Total = customer.Quantite * customer.Prix;
                db.Liste_produit.Add(customer);
                db.SaveChanges();
                //modiification de la qte d'article
                Art.Qte_article = Art.Qte_article + customer.Quantite;
                db.Entry(Art).State = EntityState.Modified;
                db.SaveChanges();
                //GridRouteValues() is an extension method which returns the
                //route values defining the grid state - current page, sort expression, filter etc.
                return RedirectToAction("Index", new { idm = customer.Id_operation });
            }
        }
    }
    catch {
        ModelState.AddModelError("", "l'article est incorrect");
    }
  //The model is invalid - render the current view to show any validation errors.
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

  ViewData["op"] = db.Mouvement_stock.Find(idm);

  var liste_produit = from l in db.Liste_produit.Include(l => l.Article).Include(l => l.Mouvement_stock)
                          where l.Id_operation == idm select l ;
  return View("Index", liste_produit.ToList());
 
}


        //
        // GET: /Default1/Details/5

        public ViewResult Details(short id)
        {
            Liste_produit liste_produit = db.Liste_produit.Find(id);
            return View(liste_produit);
        }

        //
        // GET: /Default1/Create

        public ActionResult Create()
        {
            ViewBag.Id_article = new SelectList(db.Article, "Id_article", "Libelle_article");
            ViewBag.Id_operation = new SelectList(db.Mouvement_stock, "ID_mouvement", "Date_mouvement");
            return View();
        } 

        //
        // POST: /Default1/Create

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
        // GET: /Default1/Edit/5
 
        public ActionResult Edit(short id)
        {
            Liste_produit liste_produit = db.Liste_produit.Find(id);
            ViewBag.Id_article = new SelectList(db.Article, "Id_article", "Libelle_article", liste_produit.Id_article);
            ViewBag.Id_operation = new SelectList(db.Mouvement_stock, "ID_mouvement", "Date_mouvement", liste_produit.Id_operation);
            return View(liste_produit);
        }

        //
        // POST: /Default1/Edit/5

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
        // GET: /Default1/Delete/5
 
        public ActionResult Delete(short id)
        {
            Liste_produit liste_produit = db.Liste_produit.Find(id);
            return View(liste_produit);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteP(short id, short idm)
        {

            var liste_produit1 = from l in db.Liste_produit.Include(l => l.Article).Include(l => l.Mouvement_stock)
                                 where l.Id_operation == idm && l.Id_article == id
                                           select l;

            Liste_produit L = liste_produit1.First();

            

            if (L.Mouvement_stock.Type_mouvement == "Sortie de stock")
            {
                L.Article.Qte_article = L.Article.Qte_article + L.Quantite;
                db.Entry(L.Article).State = EntityState.Modified;
                db.SaveChanges();

            }
            else
            {

                L.Article.Qte_article = L.Article.Qte_article - L.Quantite;
                db.Entry(L.Article).State = EntityState.Modified;
                db.SaveChanges();
            }

            db.Liste_produit.Remove(L);

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
            var liste_produit1 = from l in db.Liste_produit.Include(l => l.Article).Include(l => l.Mouvement_stock)
                                 where l.Id_operation == idm && l.Id_article == id
                                 select l;

            Liste_produit L = liste_produit1.First();
            var QteP = L.Quantite;

            //var Art = new Article();
            //Art = db.Article.Find(L.Id_article);
            //Perform model binding (fill the customer properties and validate it).
           // L.Id_article = article;
            //L.Article = db.Article.Find(article);
            if (TryUpdateModel(L))

            {
                //The model is valid - insert the customer and redisplay the grid.
                if (L.Mouvement_stock.Type_mouvement == "Sortie de stock")
                {
                    if (L.Article.Qte_article + QteP < L.Quantite)
                    {
                        ModelState.AddModelError("", "La quantité est supérieure à la quantité en stock");
                    }
                    else
                    {
                        L.Prix_Total = L.Quantite * L.Prix;
                        db.Entry(L).State = EntityState.Modified;
                        db.SaveChanges();

                        L.Article.Qte_article = L.Article.Qte_article + QteP - L.Quantite;
                        db.Entry(L.Article).State = EntityState.Modified;
                        db.SaveChanges();

                       /* Art.Qte_article = Art.Qte_article + QteP;
                        db.Entry(Art).State = EntityState.Modified;
                        db.SaveChanges();*/
                        //GridRouteValues() is an extension method which returns the
                        //route values defining the grid state - current page, sort expression, filter etc.
                        return RedirectToAction("Index", new { idm = L.Id_operation });
                    }
                }
                else
                {

                    L.Prix_Total = L.Quantite * L.Prix;
                    db.Entry(L).State = EntityState.Modified;
                    db.SaveChanges();

                    L.Article.Qte_article = L.Article.Qte_article -QteP + L.Quantite;
                    db.Entry(L.Article).State = EntityState.Modified;
                    db.SaveChanges();

                  /*  Art.Qte_article = Art.Qte_article - QteP;
                    db.Entry(Art).State = EntityState.Modified;
                    db.SaveChanges();*/

                    //GridRouteValues() is an extension method which returns the
                    //route values defining the grid state - current page, sort expression, filter etc.
                    return RedirectToAction("Index", new { idm = L.Id_operation });
                }
               
            }

            if (L.Quantite > 0)
            {
                if (L.Mouvement_stock.Type_mouvement == "Sortie de stock")
                {
                    if (L.Article.Qte_article + QteP < L.Quantite)
                    {
                        ModelState.AddModelError("", "La quantité est supérieure à la quantité en stock");
                    }
                    else
                    {
                        L.Prix_Total = L.Quantite * L.Prix;
                        db.Entry(L).State = EntityState.Modified;
                        db.SaveChanges();

                        L.Article.Qte_article = L.Article.Qte_article + QteP - L.Quantite;
                        db.Entry(L.Article).State = EntityState.Modified;
                        db.SaveChanges();

                       /* Art.Qte_article = Art.Qte_article + QteP;
                        db.Entry(Art).State = EntityState.Modified;
                        db.SaveChanges();*/

                        //GridRouteValues() is an extension method which returns the
                        //route values defining the grid state - current page, sort expression, filter etc.
                        return RedirectToAction("Index", new { idm = L.Id_operation });
                    }
                }
                else
                {

                    L.Prix_Total = L.Quantite * L.Prix;
                    db.Entry(L).State = EntityState.Modified;
                    db.SaveChanges();

                    L.Article.Qte_article = L.Article.Qte_article -QteP + L.Quantite;
                    db.Entry(L.Article).State = EntityState.Modified;
                    db.SaveChanges();

                   /* Art.Qte_article = Art.Qte_article - QteP;
                    db.Entry(Art).State = EntityState.Modified;
                    db.SaveChanges();*/

                    //GridRouteValues() is an extension method which returns the
                    //route values defining the grid state - current page, sort expression, filter etc.
                    return RedirectToAction("Index", new { idm = L.Id_operation });
                }
            }
            //The model is invalid - render the current view to show any validation errors.
            var articles = db.Article;
            ViewData["articles"] = articles.ToList();

            ViewData["op"] = db.Mouvement_stock.Find(idm);

            var liste_produit = from l in db.Liste_produit.Include(l => l.Article).Include(l => l.Mouvement_stock)
                                where l.Id_operation == 1
                                select l;
            return View("Index", liste_produit.ToList());
        }

        //
        // POST: /Default1/Delete/5

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