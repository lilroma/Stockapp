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
    public class ReglementController : Controller
    {
        //
        // GET: /Reglement/
        private STOCKONEntities db = new STOCKONEntities();

        public ActionResult Index(short id=0)
        {
           

                var reglements = from r in db.Reglement.Include(r => r.Client).Include(r => r.Facture_vente)
                                 where  r.Id_facture == id
                                 select r;
                return View(reglements.ToList());
          
            
        }

        //
        // GET: /Reglement/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Reglement/Create

        public ActionResult Create(short id = 0)
        {

            ViewData["fav"] = db.Facture_vente.Find(id);
            
            return View();
        }

        public ViewResult listclient()
        {
            var clients = from c in db.Client
                          select c;
            return View(clients.ToList());

        }

        [HttpPost]
        public ViewResult listclient(string searchString)
        {
            var clients = from c in db.Client
                          select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(c => c.Nom_client.ToUpper().Contains(searchString.ToUpper())
                                       || c.Prenom_client.ToUpper().Contains(searchString.ToUpper()));
            }
            return View(clients.ToList());
        
        }

        public ViewResult ListeFacture(short id)

        {

            var listefacture = from l in db.Facture_vente.Include(l => l.Liste_produit_vente).Include(l => l.Reglement)
                               where l.Id_client == id
                               select l;
            var listefactures = new List<Facture_vente>();


            foreach(var l in listefacture)
            {
                double sum = 0;
                double sum1 = 0; 

                foreach (var p in l.Liste_produit_vente)
                {
                    sum = sum + double.Parse(p.Prix.ToString());
                }

                foreach (var r in l.Reglement)
                {
                    sum1 = sum1 + double.Parse(r.Montant_regelement.ToString());
                }

                if (sum1 < sum)
                {
                    listefactures.Add(l);
                }

            }
            return View(listefactures.ToList());

        }

        //
        // POST: /Reglement/Create

        [HttpPost]
        public ActionResult Create(Reglement reglement, string idf, string idc)
        {
            if (ModelState.IsValid)
            {
                reglement.Id_facture = short.Parse(idf);
                reglement.Id_client = short.Parse(idc);

                db.Reglement.Add(reglement);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = reglement.Id_facture });
            }

            return View(reglement);
        }
        
        //
        // GET: /Reglement/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Reglement/Edit/5

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
        // GET: /Reglement/Delete/5
 
        public ActionResult Delete(short id, short idc)
        {
            var reglement = db.Reglement.Find(id,idc);

            return View(reglement);
        }

        //
        // POST: /Reglement/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id, short idc)
        {
            Reglement reglement = db.Reglement.Find(id);
            db.Reglement.Remove(reglement);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = id });
        }

    }
}
