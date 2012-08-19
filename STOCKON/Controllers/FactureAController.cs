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
    public class FactureAController : Controller
    {
        //
        // GET: /FactureA/
        private STOCKONEntities db = new STOCKONEntities();
        public ActionResult Index()
        {
            return View(db.Facture_achat.ToList());
        }

        //
        // GET: /FactureA/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /FactureA/Create

        public ActionResult Create()
        {
            //ViewBag.Id_fournisseur = new SelectList(db.Fournisseur, "Id_fournisseur", "Nom_fournisseur");
            var fournisseurs = db.Fournisseur.ToList();
            Facture_achat facture_achat = new Facture_achat();

            List<String> listToBind = new List<String>();

            foreach (var item in fournisseurs)
            {
                listToBind.Add(item.Nom_fournisseur + " " + item.Prenom_fournisseur + " - " + item.Id_fournisseur);
            }

            ViewData["Fournisseur"] = listToBind;
            var fa = db.Facture_achat.Max(a => a.Id_facture) + 1;

            facture_achat.Code_facture = MyGlobalVariables.FormatCode("FAA", fa);
            return View(facture_achat);
        } 

        //
        // POST: /FactureA/Create

        [HttpPost]
        public ActionResult Create(Facture_achat fac, string Mrg, string TypeF, string test)
        {
            fac.Mode_reglement = Mrg;
            fac.Type_facture = TypeF;
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        int px = test.IndexOf("-") + 2;
                        int p = test.Length - px;
                        string v = test.Substring(px, p);
                        fac.Id_fournisseur = short.Parse(v);

                        db.Facture_achat.Add(fac);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Liste_produit_achat", new { idm = fac.Id_facture });
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Le fournisseur est incorrect");
                    }

                }
                //ViewBag.Id_fournisseur = new SelectList(db.Fournisseur, "Id_fournisseur", "Nom_fournisseur");
                var fournisseurs = db.Fournisseur.ToList();

                List<String> listToBind = new List<String>();

                foreach (var item in fournisseurs)
                {
                    listToBind.Add(item.Nom_fournisseur + " " + item.Prenom_fournisseur + " - " + item.Id_fournisseur);
                }

                ViewData["Fournisseur"] = listToBind;
                
                return View();
               
            }
            catch
            {
                var fournisseurs = db.Fournisseur.ToList();

                List<String> listToBind = new List<String>();

                foreach (var item in fournisseurs)
                {
                    listToBind.Add(item.Nom_fournisseur + " " + item.Prenom_fournisseur + " - " + item.Id_fournisseur);
                }

                ViewData["Fournisseur"] = listToBind;
                // ViewBag.Id_fournisseur = new SelectList(db.Fournisseur, "Id_fournisseur", "Nom_fournisseur");
                return View();
            }
        }
        
        //
        // GET: /FactureA/Edit/5
 
        public ActionResult Edit(int id)
        {
            Facture_achat facture_achat = new Facture_achat();
            facture_achat = db.Facture_achat.Find(id);

            if (facture_achat.Type_facture.Trim() == "Facture")
                facture_achat.Type_facture = "Facture comptabilisée";
            
            return View(facture_achat);
        }

        //
        // POST: /FactureA/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, Facture_achat facture_achat, string type="")
        {
           Facture_achat fa = new Facture_achat();
          try
           {
           fa = db.Facture_achat.Find(facture_achat.Id_facture);
           if (type == "")
               fa.Type_facture = facture_achat.Type_facture;
           else
           {
              if (fa.Type_facture == "Bon de commande")
              {
                var lp = from l in db.Liste_produit_achat.Include(l => l.Article)
                         where l.Id_facture == fa.Id_facture
                         select l;
                 IList<Liste_produit_achat> ListP = lp.ToList();
                 // using (var context = new STOCKONEntities())
                //  {
                  foreach (Liste_produit_achat listep in ListP)
                  {
                      Article article = new Article();
                      article = listep.Article;
                    article.Qte_article = article.Qte_article + listep.Quantite;
                    db.Entry(article).State = EntityState.Modified;
                    db.SaveChanges();
                  }
              //    }
              
              }

              fa.Type_facture = type;
              
               
               }

                db.Entry(fa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
           }
         catch
            {
                return View(fa);
            }
        }

        //
        // GET: /FactureA/Delete/5
        public ActionResult Delete(short id)
        {
            Facture_achat fa = db.Facture_achat.Find(id);
            return View(fa);
        }
       
        //
        // POST: /FactureA/Delete/5
         [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var listp = from l in db.Liste_produit_achat
                            where l.Id_facture == id
                            select l;
                if (listp.Count() == 0)
                {
                    Facture_achat fa = db.Facture_achat.Find(id);
                    db.Facture_achat.Remove(fa);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    Facture_achat fa = db.Facture_achat.Find(id);
                    ModelState.AddModelError("","Suppression impossible");
                    return View(fa);
                }
            }
            catch
            {
                Facture_achat fa = db.Facture_achat.Find(id);
                ModelState.AddModelError("", "Suppression impossible");
                return View(fa);
            }
        }

        public ActionResult AddProduit(short id)
        {
          
            return RedirectToAction("Index", "Liste_produit_achat", new { idm = id });
        }

        
    }
}
