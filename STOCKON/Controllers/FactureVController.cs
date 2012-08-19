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
    public class FactureVController : Controller
    {
        //
        // GET: /FactureA/
        private STOCKONEntities db = new STOCKONEntities();
        public ActionResult Index()
        {
            return View(db.Facture_vente.ToList());
        }

        //
        // GET: /FactureA/Details/5

        public ActionResult Details(short id)
        {
            return View();
        }

        //
        // GET: /FactureA/Create

        public ActionResult Create()
        {
            //ViewBag.Id_client = new SelectList(db.Client, "Id_client", "Nom_client");
            var clients = db.Client.ToList();
            Facture_vente facture_vente = new Facture_vente();

            List<String> listToBind = new List<String>();

            foreach (var item in clients)
            {
                listToBind.Add(item.Nom_client + " " + item.Prenom_client + " - " + item.Id_client);
            }

            var fa = db.Facture_vente.Max(a => a.Id_facture) + 1;

            facture_vente.Code_facture = MyGlobalVariables.FormatCode("FAV", fa);

            ViewData["Client"] = listToBind;
            return View(facture_vente);
        }

        public ActionResult Reglement(short id)
        {
            return RedirectToAction("Index", "Reglement", new {id = id });
        
        }
        //
        // POST: /FactureA/Create

        [HttpPost]
        public ActionResult Create(Facture_vente fac, string Mrg, string TypeF, string test)
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
                        fac.Id_client = short.Parse(v);
                        fac.Remise = 0;
                        fac.TVA = false;

                        db.Facture_vente.Add(fac);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Liste_produit_vente", new { idm = fac.Id_facture });
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Le client est incorrect");
                    }

                }
               // ViewBag.Id_client = new SelectList(db.Client, "Id_client", "Nom_client");
                var clients = db.Client.ToList();

                List<String> listToBind = new List<String>();

                foreach (var item in clients)
                {
                    listToBind.Add(item.Nom_client + " " + item.Prenom_client + " - " + item.Id_client);
                }

                ViewData["Client"] = listToBind;
                return View();


            }
            catch
            {
               // ViewBag.Id_client = new SelectList(db.Client, "Id_client", "Nom_client");
                var clients = db.Client.ToList();

                List<String> listToBind = new List<String>();

                foreach (var item in clients)
                {
                    listToBind.Add(item.Nom_client + " " + item.Prenom_client + " - " + item.Id_client);
                }

                ViewData["Client"] = listToBind;
                return View();
            }
        }

        //
        // GET: /FactureA/Edit/5

        public ActionResult Edit(int id)
        {
            Facture_vente facture_vente = new Facture_vente();
            facture_vente = db.Facture_vente.Find(id);

            if (facture_vente.Type_facture.Trim() == "Facture")
                facture_vente.Type_facture = "Facture comptabilisée";
            return View(facture_vente);
        }

        //
        // POST: /FactureA/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, Facture_vente facture_vente, string type)
        {
            Facture_vente fa = new Facture_vente();
            try
           {
                fa = db.Facture_vente.Find(facture_vente.Id_facture);
                if (facture_vente.Type_facture == "Facture comptabilisée")
                {
                    fa.Type_facture = facture_vente.Type_facture;
                    db.Entry(fa).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    if (fa.Type_facture == "Bon de commande")
                    {
                        Article article = new Article();
                        
                        var lp = from l in db.Liste_produit_vente.Include(l => l.Article)
                                 where l.Id_facture == fa.Id_facture
                                 select l;

                        if (VerifQ(lp.ToList()) == true)
                        {
                            ModelState.AddModelError("", "Les quantités en stock ne permettent pas de satisfaire cette démande : transformation impossible");
                            return View(fa);
                        }
                        else
                        {
                            IList<Liste_produit_vente> ListeP = lp.ToList();
                        foreach (Liste_produit_vente listep in ListeP)
                        {
                            listep.Article.Qte_article = listep.Article.Qte_article - listep.Quantite;
                            db.Entry(listep.Article).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        fa.Type_facture = type;
                        db.Entry(fa).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                            
                        }

                        return RedirectToAction("Index");

                    }
                    else
                    {
                        fa.Type_facture = type;
                        db.Entry(fa).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }


                }
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
            Facture_vente fa = db.Facture_vente.Find(id);
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
                var reglement = from r in db.Reglement
                                 where r.Id_facture == id
                                 select r;
                var listp = from l in db.Liste_produit_vente
                            where l.Id_facture == id
                            select l;

                if (reglement.Count() == 0 && listp.Count() == 0 )
                {
                    Facture_vente fa = db.Facture_vente.Find(id);
                    db.Facture_vente.Remove(fa);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    Facture_vente fa = db.Facture_vente.Find(id);
                    ModelState.AddModelError("", "Suppression impossible");
                    return View(fa);
                }
            }
            catch
            {
                Facture_vente fa = db.Facture_vente.Find(id);
                ModelState.AddModelError("", "Suppression impossible");
                return View(fa);
            }
        }

        public ActionResult AddProduit(short id)
        {

            return RedirectToAction("Index", "Liste_produit_vente", new { idm = id });
        }

        public Boolean VerifQ(List<Liste_produit_vente> liste_produit_ventes)
        {
            int i = 0;
            Boolean result = false;
            while (i < liste_produit_ventes.Count && result == false)
            {
                if (liste_produit_ventes.ElementAt(i).Article.Qte_article < liste_produit_ventes.ElementAt(i).Quantite)
                    result = true;
                i = i + 1;
            }
            return result;
        }


    }
}
