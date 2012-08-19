using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STOCKON.Models;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.UI;
using Telerik.Web.Mvc.Configuration;

namespace STOCKON.Controllers
{ 
    public class Mouvement_stockController : Controller
    {
        private STOCKONEntities db = new STOCKONEntities();

        //
        // GET: /Mouvement_stock/

        public ViewResult Index()
        {
            return View(db.Mouvement_stock.ToList());
        }

        public ViewResult Test()
        {
            return View();
        }
        //
        // GET: /Mouvement_stock/Details/5

        public ViewResult Details(short id)
        {
            Mouvement_stock mouvement_stock = db.Mouvement_stock.Find(id);
            return View(mouvement_stock);
        }

        //
        // GET: /Mouvement_stock/Create

        public ActionResult Create()
        {
            Mouvement_stock mouvement_stock = new Mouvement_stock();

            var mv = db.Mouvement_stock.Max(a => a.ID_mouvement) + 1;

            mouvement_stock.Code_mouvement = MyGlobalVariables.FormatCode("MVS", mv);
            
            return View(mouvement_stock);
        } 

        //
        // POST: /Mouvement_stock/Create

        [HttpPost]
        public ActionResult Create(Mouvement_stock mouvement_stock, string TypeM)
        {
            mouvement_stock.Type_mouvement = TypeM;
            if (ModelState.IsValid)
            {
                db.Mouvement_stock.Add(mouvement_stock);
                db.SaveChanges();
                return RedirectToAction("Index", "Default1", new { idm = mouvement_stock.ID_mouvement });
               // return RedirectToAction("AddProduit",new {idm = mouvement_stock.ID_mouvement });  
                
            }


            return View(mouvement_stock);
        }
        
        //
        // GET: /Mouvement_stock/Edit/5
 
        public ActionResult Edit(short id)
        {
            Mouvement_stock mouvement_stock = db.Mouvement_stock.Find(id);
            return View(mouvement_stock);
        }

        //
        // POST: /Mouvement_stock/Edit/5

        [HttpPost]
        public ActionResult Edit(Mouvement_stock mouvement_stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mouvement_stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mouvement_stock);
        }

        //
        // GET: /Mouvement_stock/Delete/5
 
        public ActionResult Delete(short id)
        {
            Mouvement_stock mouvement_stock = db.Mouvement_stock.Find(id);
            return View(mouvement_stock);
        }

        //
        // POST: /Mouvement_stock/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {
            var ListP = from l in db.Liste_produit
                        where l.Id_operation == id
                        select l;
            if (ListP.Count() > 0)
            {
                Mouvement_stock mouvement_stock = db.Mouvement_stock.Find(id);
                ModelState.AddModelError("", "Supression impossible");   
                return View(mouvement_stock);
            }
            else
            {
                Mouvement_stock mouvement_stock = db.Mouvement_stock.Find(id);
                db.Mouvement_stock.Remove(mouvement_stock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }


         [GridAction]
        public ActionResult AjaxBinding(/*short id, string datem, string libelle, string type*/)
        {
             
            return View(new GridModel(db.Liste_produit.ToList()));
        }

        public ActionResult AddProduit(short id)
        {
          /*
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
                        Article = new Arti { 
                         Id_article = fr.Article.Id_article,
                         Libelle_article = fr.Article.Libelle_article,
                        }
                        
                    };
            
           // ViewData["Liste_Produit"] = f.ToList(); 

          //  ViewData["Liste_Produit"] = db.Liste_produit.ToList(); 
           ViewData["op"] = db.Mouvement_stock.Find(id);
            var art = from ar in db.Article
                      select new Arti{
                        Id_article = ar.Id_article,
                        Libelle_article = ar.Libelle_article,
                      };

           ViewData["Article"] = art.ToList();
            
            return View(f.ToList());*/

            return RedirectToAction("Index", "Default1", new { idm = id });
        }

         [GridAction]
        public ActionResult Edition()
        {
            var art = from ar in db.Article
                      select new Arti
                      {
                          Id_article = ar.Id_article,
                          Libelle_article = ar.Libelle_article,
                      };

            ViewData["Article"] = art.ToList();

            return View();
        }
         [AcceptVerbs(HttpVerbs.Post)]
         [GridAction]
         public ActionResult _Create(short id, short Article)
         {

             ProduitL produitl = new ProduitL();

             if (TryUpdateModel(produitl, null, null, new[] { "Article" }))
             {
                 Liste_produit liste_produit = new Liste_produit{

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

         public ActionResult EditingAjax(GridEditMode? mode, GridButtonType? type)
         {
             ViewData["mode"] = mode ?? GridEditMode.InLine;
             ViewData["type"] = type ?? GridButtonType.Text;
             return View();
         }

        //
        // POST: /Mouvement_stock/Create

     /*   [HttpPost]
        public ActionResult AddProduit(Liste_produit liste_produit)
        {
            if (ModelState.IsValid)
            {
                //db.Liste_produit.Add(liste_produit);
              //  db.SaveChanges();
               // return RedirectToAction("Index");
            }

            return View(liste_produit.Mouvement_stock);
        } */

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}