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
    public class FournisseurController : Controller
    {
        private STOCKONEntities db = new STOCKONEntities();

        //
        // GET: /Fournisseur/

        public ViewResult Index(string searchString)
        {
            var f = from fr in db.Fournisseur
                          select fr;

            if (!String.IsNullOrEmpty(searchString))
            {
                f = f.Where(fr => fr.Nom_fournisseur.ToUpper().Contains(searchString.ToUpper())
                                       || fr.Prenom_fournisseur.ToUpper().Contains(searchString.ToUpper()));
            }
            return View(f.ToList());
        }

        //
        // GET: /Fournisseur/Details/5


        public ViewResult Details(short id)
        {
            Fournisseur fournisseur = db.Fournisseur.Find(id);
            return View(fournisseur);
        }

        //
        // GET: /Fournisseur/Create

        public ActionResult Create()
        {
            Fournisseur fournisseur = new Fournisseur();

            var frs = db.Fournisseur.Max(a => a.Id_fournisseur) + 1;

            fournisseur.Code_fournisseur = MyGlobalVariables.FormatCode("FRS", frs);
            
            return View(fournisseur);
        } 

        //
        // POST: /Fournisseur/Create

        [HttpPost]
        public ActionResult Create(Fournisseur fournisseur)
        {
            if (ModelState.IsValid)
            {
                var fr = from f in db.Fournisseur
                         where f.Code_fournisseur == fournisseur.Code_fournisseur
                         select f;
                if (fr.ToList().Count > 0)
                {
                    ModelState.AddModelError("", "Le code que vous avez specifié est déjà utilisé");
                }
                else
                {
                    db.Fournisseur.Add(fournisseur);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(fournisseur);
        }
        
        //
        // GET: /Fournisseur/Edit/5
 
        public ActionResult Edit(short id)
        {
            Fournisseur fournisseur = db.Fournisseur.Find(id);
            return View(fournisseur);
        }

        //
        // POST: /Fournisseur/Edit/5

        [HttpPost]
        public ActionResult Edit(Fournisseur fournisseur)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fournisseur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fournisseur);
        }

        //
        // GET: /Fournisseur/Delete/5
 
        public ActionResult Delete(short id)
        {
            Fournisseur fournisseur = db.Fournisseur.Find(id);
            return View(fournisseur);
        }

        //
        // POST: /Fournisseur/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {            
            try
            {
            Fournisseur fournisseur = db.Fournisseur.Find(id);
            db.Fournisseur.Remove(fournisseur);
            db.SaveChanges();
            return RedirectToAction("Index");
            }
            catch (Exception Ex)
            {
                ModelState.AddModelError("", "Suppression impossible");
                return View();

            }
        }

        public ViewResult Lachat(short id)
        {
            var liste_produit_achats = from l in db.Liste_produit_achat.Include(a => a.Article).Include(a => a.Facture_achat).Include(a => a.Facture_achat.Fournisseur)
                                       where l.Facture_achat.Id_fournisseur == id
                                       select l;

            return View(liste_produit_achats.ToList());

        }


        public ViewResult Lfacture(short id)
        {
            var factures = from l in db.Facture_achat.Include(a => a.Liste_produit_achat).Include(a => a.Fournisseur)
                           where l.Id_fournisseur == id
                           select l;

            return View(factures.ToList());

        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}