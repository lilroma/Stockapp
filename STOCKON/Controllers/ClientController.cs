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
    public class ClientController : Controller
    {
        private STOCKONEntities db = new STOCKONEntities();

        //
        // GET: /Client/

        public ViewResult Index(string searchString)
        {
           var clients = from c in db.Client
                   select c;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(c => c.Nom_client .ToUpper().Contains(searchString.ToUpper())
                                       || c.Prenom_client.ToUpper().Contains(searchString.ToUpper()));
            }
           
            return View(clients.ToList());
        }

        public ViewResult Lvente(short id)
        {
            var liste_produit_ventes = from l in db.Liste_produit_vente.Include(a => a.Article).Include(a => a.Facture_vente).Include(a => a.Facture_vente)
                                       where l.Facture_vente.Id_client == id
                                       select l;

            return View(liste_produit_ventes.ToList());

        }


        public ViewResult Lfacture(short id)
        {
            var factures = from l in db.Facture_vente.Include(a => a.Liste_produit_vente).Include(a => a.Client)
                                       where l.Id_client == id
                                       select l;

            return View(factures.ToList());

        }

        public ViewResult Lreglement(short id)
        {

            var listefacture = from l in db.Facture_vente.Include(l => l.Liste_produit_vente).Include(l => l.Reglement)
                               where l.Id_client == id
                               select l;
            var listefactures = new List<Facture_vente>();


            foreach (var l in listefacture)
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
        // GET: /Client/Details/5

        public ViewResult Details(short id)
        {
            Client client = db.Client.Find(id);
            return View(client);
        }

        //
        // GET: /Client/Create

        public ActionResult Create()
        {
            Client client = new Client();

            var cli = db.Client.Max(a => a.Id_client) + 1;

            client.Code_client = MyGlobalVariables.FormatCode("CLT", cli); 
            return View(client);
        } 

        //
        // POST: /Client/Create

        [HttpPost]
        public ActionResult Create(Client client, string Categorie)
        {
            if (ModelState.IsValid)
            {
                var clt = from c in db.Client
                         where c.Code_client == client.Code_client
                         select c;
                if (clt.ToList().Count > 0)
                {
                    ModelState.AddModelError("", "Le code que vous avez specifié est déjà utilisé");
                }
                else
                {
                    client.Categorie_client = Categorie;
                    db.Client.Add(client);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(client);
        }
        
        //
        // GET: /Client/Edit/5
 
        public ActionResult Edit(short id)
        {
            Client client = db.Client.Find(id);
            return View(client);
        }

        //
        // POST: /Client/Edit/5

        [HttpPost]
        public ActionResult Edit(Client client, string Categorie)
        {
            if (ModelState.IsValid)
            {
                client.Categorie_client = Categorie;
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        //
        // GET: /Client/Delete/5
 
        public ActionResult Delete(short id)
        {
            Client client = db.Client.Find(id);
            return View(client);
        }

        //
        // POST: /Client/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {
            try
            {
                Client client = db.Client.Find(id);
                db.Client.Remove(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception Ex)
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