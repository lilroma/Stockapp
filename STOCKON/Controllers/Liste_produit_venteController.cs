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
    public class Liste_produit_venteController : Controller
    {
        //
        // GET: /Liste_produit_vente/
        private STOCKONEntities db = new STOCKONEntities();

        public ActionResult Index(string Remise, Boolean TVA = false, short idm = 0)
        {
            if (idm == 0)
                idm = STOCKON.MyGlobalVariables.fvid;
            else
                STOCKON.MyGlobalVariables.fvid = idm;

            var article = db.Article;
            ViewData["articles"] = article.ToList();

            if (!String.IsNullOrEmpty(Remise))

            {
                Facture_vente Fv = new Facture_vente();
                Fv = db.Facture_vente.Find(idm);
               
                try
                {
                    double R = Convert.ToDouble(Remise);
                    if (db.Preference.Find(1).Remise.Trim() == "Pourcentage".Trim())
                    {
                        if (R > 100)
                            ModelState.AddModelError("", "La remise doit être inférieure à 100");
                        else
                        {
                            Fv.Remise = R;
                            Fv.TVA = TVA;
                            db.Entry(Fv).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                    else
                    {
                        Fv.Remise = R;
                        Fv.TVA = TVA;
                        db.Entry(Fv).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "La remise doit être un montant positif");
                }
            }

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

            ViewData["Fa"] = db.Facture_vente.Find(idm);

            var liste_produit = from l in db.Liste_produit_vente.Include(l => l.Article).Include(l => l.Facture_vente)
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

            ViewData["Fa"] = db.Facture_vente.Find(idm);

            var liste_produit = from l in db.Liste_produit_vente.Include(l => l.Article).Include(l => l.Facture_vente)
                                where l.Id_facture == idm
                                select l;
            return View(liste_produit.ToList());
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Insert(short idm, string article)
        {
            ModelState.Clear();

            try
            {

            int px = article.IndexOf("-") + 2;
            int p = article.Length - px;
            string v = article.Substring(px, p);
            
            //Create a new instance of the Customer class.
            Liste_produit_vente customer = new Liste_produit_vente
            {
                Id_article = short.Parse(v),
                Id_facture = idm
            };
            Facture_vente Fa = new Facture_vente();
            Fa = db.Facture_vente.Find(idm);

                Article Art = new Article();
                Art = db.Article.Find(customer.Id_article);

                Client client = new Client();

                client = db.Client.Find(db.Facture_vente.Find(idm).Id_client);

                //Perform model binding (fill the customer properties and validate it).
                if (TryUpdateModel(customer))
                {
                    if (Fa.Type_facture.Trim() == "Bon de commande".Trim())
                    {
                        if (client.Categorie_client.Trim() == "Grossiste".Trim())
                        {
                            customer.Prix = Art.Prix_grossiste;
                        }
                        else
                        {
                            customer.Prix = Art.Prix_detaillant;
                        }
                        customer.Prix_Total = customer.Prix * customer.Quantite;
                        db.Liste_produit_vente.Add(customer);
                        db.SaveChanges();
                        return RedirectToAction("Index", new { idm = customer.Id_facture });
                    }
                    else
                    {
                        //The model is valid - insert the customer and redisplay the grid.
                        if (Art.Qte_article < customer.Quantite)
                        {
                            ModelState.AddModelError("", "La quantité est supérieure à la quantité en stock");
                        }
                        else
                        {

                            if (client.Categorie_client.Trim() == "Grossiste".Trim())
                            {
                                customer.Prix = Art.Prix_grossiste;
                            }
                            else
                            {
                                customer.Prix = Art.Prix_detaillant;
                            }

                            customer.Prix_Total = customer.Prix * customer.Quantite;
                            db.Liste_produit_vente.Add(customer);
                            db.SaveChanges();
                            //modiification de la qte d'article
                            Art.Qte_article = Art.Qte_article - customer.Quantite;
                            db.Entry(Art).State = EntityState.Modified;
                            db.SaveChanges();
                            //GridRouteValues() is an extension method which returns the
                            //route values defining the grid state - current page, sort expression, filter etc.
                            return RedirectToAction("Index", new { idm = customer.Id_facture });
                        }
                    }
                }

                if (customer.Quantite > 0)
                {
                    if (Fa.Type_facture.Trim() == "Bon de commande".Trim())
                    {
                        if (client.Categorie_client.Trim() == "Grossiste".Trim())
                        {
                            customer.Prix = Art.Prix_grossiste;
                        }
                        else
                        {
                            customer.Prix = Art.Prix_detaillant;
                        }
                        customer.Prix_Total = customer.Prix * customer.Quantite;
                        db.Liste_produit_vente.Add(customer);
                        db.SaveChanges();
                        return RedirectToAction("Index", new { idm = customer.Id_facture });
                    }
                    else
                    {
                        if (Art.Qte_article < customer.Quantite)
                        {
                            ModelState.AddModelError("", "La quantité est supérieure à la quantité en stock");
                        }
                        else
                        {
                            if (client.Categorie_client.Trim() == "Grossiste".Trim())
                            {
                                customer.Prix = Art.Prix_grossiste;
                            }
                            else
                            {
                                customer.Prix = Art.Prix_detaillant;
                            }

                            customer.Prix_Total = customer.Prix * customer.Quantite;
                            db.Liste_produit_vente.Add(customer);
                            db.SaveChanges();
                            //modiification de la qte d'article
                            Art.Qte_article = Art.Qte_article - customer.Quantite;
                            db.Entry(Art).State = EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Index", new { idm = customer.Id_facture });
                        }
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

            ViewData["Fa"] = db.Facture_vente.Find(idm);

            var liste_produit = from l in db.Liste_produit_vente.Include(l => l.Article).Include(l => l.Facture_vente)
                                where l.Id_facture == idm
                                select l;
            return View("index", liste_produit.ToList());

        }

        public ActionResult Reglement(short id)
        {
            return RedirectToAction("Index", "Reglement", new { id = id });

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteP(short id, short idm)
        {

            var liste_produit1 = from l in db.Liste_produit_vente.Include(l => l.Article).Include(l => l.Facture_vente)
                                 where l.Id_facture == idm && l.Id_article == id
                                 select l;

            Liste_produit_vente L = liste_produit1.First();

            Facture_vente Fa = new Facture_vente();
           Fa = db.Facture_vente.Find(idm);

             if (Fa.Type_facture.Trim() == "Bon de commande".Trim())
             {
                    }
                    else
                    {
            L.Article.Qte_article = L.Article.Qte_article + L.Quantite;
            db.Entry(L.Article).State = EntityState.Modified;
            db.SaveChanges();
             }

            db.Liste_produit_vente.Remove(L);
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
            var liste_produit1 = from l in db.Liste_produit_vente.Include(l => l.Article).Include(l => l.Facture_vente)
                                 where l.Id_facture == idm && l.Id_article == id
                                 select l;
            Facture_vente Fa = new Facture_vente();
            Fa = db.Facture_vente.Find(idm);

            Liste_produit_vente L = liste_produit1.First();
            var QteP = L.Quantite;
            var Prixp = L.Prix;

           // var Art = new Article();

            //Art = db.Article.Find(L.Id_article);
            //Perform model binding (fill the customer properties and validate it).
            //L.Id_article = article;
            if (TryUpdateModel(L))
            {
                if (Fa.Type_facture.Trim() == "Bon de commande".Trim())
                {
                    L.Prix = Prixp;
                    L.Prix_Total = L.Quantite * L.Prix;
                    db.Entry(L).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { idm = L.Id_facture });
                }
                else
                {
                    if (L.Article.Qte_article + QteP < L.Quantite)
                    {
                        ModelState.AddModelError("", "La quantité est supérieure à la quantité en stock");
                    }
                    else
                    {
                        if ((L.Article.Qte_article + QteP - L.Quantite) <= L.Article.Stock_critique)
                        {
                            ModelState.AddModelError("", "La quantité en stock pour cet article est en dessous du sueil critique");
                        }

                        L.Prix = Prixp;
                        L.Prix_Total = L.Quantite * L.Prix;
                        db.Entry(L).State = EntityState.Modified;
                        db.SaveChanges();
                        L.Article.Qte_article = (L.Article.Qte_article + QteP - L.Quantite);
                        db.Entry(L.Article).State = EntityState.Modified;
                        db.SaveChanges();

                        /* Art.Qte_article = Art.Qte_article + QteP;
                         db.Entry(Art).State = EntityState.Modified;
                         db.SaveChanges();*/
                        //GridRouteValues() is an extension method which returns the
                        //route values defining the grid state - current page, sort expression, filter etc.
                        return RedirectToAction("Index", new { idm = L.Id_facture });
                    }
                }
            }

            if (L.Quantite > 0)
            {
                if (Fa.Type_facture.Trim() == "Bon de commande".Trim())
                {
                    L.Prix = Prixp;
                    L.Prix_Total = L.Quantite * L.Prix;
                    db.Entry(L).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { idm = L.Id_facture });
                }
                else
                {
                    if (L.Article.Qte_article + QteP < L.Quantite)
                    {
                        ModelState.AddModelError("", "La quantité est supérieure à la quantité en stock");
                    }
                    else
                    {
                        if ((L.Article.Qte_article + QteP - L.Quantite) <= L.Article.Stock_critique)
                        {
                            ModelState.AddModelError("", "La quantité en stock pour cet article est en dessous du sueil critique");
                        }

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
                        return RedirectToAction("Index", new { idm = L.Id_facture });
                    }
                }
            }
            //The model is invalid - render the current view to show any validation errors.
            var articles = db.Article;
            ViewData["articles"] = articles.ToList();

            ViewData["Fa"] = db.Facture_vente.Find(idm);

            var liste_produit = from l in db.Liste_produit_vente.Include(l => l.Article).Include(l => l.Facture_vente)
                                where l.Id_facture == idm
                                select l;
            return View("Index", liste_produit.ToList());
        }

        //
        // GET: /Liste_produit_vente/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Liste_produit_vente/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Liste_produit_vente/Create

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
        // GET: /Liste_produit_vente/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Liste_produit_vente/Edit/5

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
        // GET: /Liste_produit_vente/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Liste_produit_vente/Delete/5

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
