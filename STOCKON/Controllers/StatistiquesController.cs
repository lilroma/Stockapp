using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STOCKON.Models;
using System.Web.Helpers;

namespace STOCKON.Controllers
{
    public class StatistiquesController : Controller
    {
        //
        // GET: /Statistiques/

        private STOCKONEntities db = new STOCKONEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StatVente()
        {

            return View(); 
        }

        [HttpPost]
        public ActionResult StatVente(DateModel datemodel, string Type)
        {

            datemodel.Type = Type;
            MyGlobalVariables.datem = datemodel;
            return RedirectToAction("StatVente", new { DateD = datemodel.DateD, DateF = datemodel.DateF, Type = Type });    
          //return View();
        }


        public ActionResult VStatv(/*DateTime DateD, DateTime DateF, string Type*/)
        {

            List<Liste_produit_vente> listeproduit = new List<Liste_produit_vente>(); 

        var listeproduits = from l in db.Liste_produit_vente.Include(l => l.Article).Include(l => l.Facture_vente)
                         where l.Facture_vente.Date_facture >= MyGlobalVariables.datem.DateD && l.Facture_vente.Date_facture <= MyGlobalVariables.datem.DateF
                              select l;

        listeproduit = listeproduits.ToList();

        int[] Qte = new int[listeproduit.Count()];
        string[] Lib = new string[listeproduit.Count()];
        int i = 0;

            foreach ( Liste_produit_vente l in listeproduit)
            {
                Qte[i] = int.Parse(l.Quantite.ToString());
                Lib[i] = l.Article.Libelle_article;
                i = i + 1;

            }

            // ViewData["LP"] = listeproduit.ToList();
            if (/*Request.Form["Type"].Trim()*/ MyGlobalVariables.datem.Type == "Diagramme en secteur")
            {
                var Mychart = new Chart(width: 800, height: 600, theme: ChartTheme.Vanilla3D)
         .AddTitle("Ventes du " + MyGlobalVariables.datem.DateD.Date.ToShortDateString() + " au " + MyGlobalVariables.datem.DateF.ToShortDateString())
         .AddSeries("Default", chartType: "pie",
                      xValue: Lib,
                      yValues: Qte)
             // xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
               //     yValues: new[] { "20", "20", "40", "10", "10" })
          .GetBytes("png");
                return File(Mychart, "image/png");
            }
            else
            {
                var Mychart = new Chart(width: 800, height: 600, theme: ChartTheme.Green)
          .AddTitle("Ventes du " + MyGlobalVariables.datem.DateD.Date.ToShortDateString() + " au " + MyGlobalVariables.datem.DateF.ToShortDateString())
          .AddSeries("Default", 
                      xValue: Lib,
                      yValues: Qte)
                    // xValue: listeproduit, xField: "Article.Libelle_article",
                    // yValues: listeproduit, yFields: "Quantite")
               // xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                 //   yValues: new[] { "20", "20", "40", "10", "10" })
           .GetBytes("png");
               
               return File(Mychart, "image/png");
                
            }
        
        }

        public ActionResult StatAchat()
        {

            return View();
        }

        [HttpPost]
        public ActionResult StatAchat(DateModel datemodel, string Type)
        {

            datemodel.Type = Type;
            MyGlobalVariables.datem = datemodel;
            return RedirectToAction("StatAchat", new { DateD = datemodel.DateD, DateF = datemodel.DateF, Type = Type });
            //return View();
        }


        public ActionResult VStata(/*DateTime DateD, DateTime DateF, string Type*/)
        {

            List<Liste_produit_achat> listeproduit = new List<Liste_produit_achat>();

            var listeproduits = from l in db.Liste_produit_achat.Include(l => l.Article).Include(l => l.Facture_achat)
                                where l.Facture_achat.Date_facture >= MyGlobalVariables.datem.DateD && l.Facture_achat.Date_facture <= MyGlobalVariables.datem.DateF
                                select l;

            listeproduit = listeproduits.ToList();

            int[] Qte = new int[listeproduit.Count()];
            string[] Lib = new string[listeproduit.Count()];
            int i = 0;

            foreach (Liste_produit_achat l in listeproduit)
            {
                Qte[i] = int.Parse(l.Quantite.ToString());
                Lib[i] = l.Article.Libelle_article;
                i = i + 1;

            }

            // ViewData["LP"] = listeproduit.ToList();
            if (/*Request.Form["Type"].Trim()*/ MyGlobalVariables.datem.Type == "Diagramme en secteur")
            {
                var Mychart = new Chart(width: 800, height: 600, theme: ChartTheme.Vanilla3D)
         .AddTitle("Achats du " + MyGlobalVariables.datem.DateD.Date.ToShortDateString() + " au " + MyGlobalVariables.datem.DateF.ToShortDateString())
         .AddSeries("Default", chartType: "pie",
                      xValue: Lib,
                      yValues: Qte)
                    // xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                    //     yValues: new[] { "20", "20", "40", "10", "10" })
          .GetBytes("png");
                return File(Mychart, "image/png");
            }
            else
            {
                var Mychart = new Chart(width: 800, height: 600, theme: ChartTheme.Green)
          .AddTitle("Achats du " + MyGlobalVariables.datem.DateD.Date.ToShortDateString() + " au " + MyGlobalVariables.datem.DateF.ToShortDateString())
          .AddSeries("Default",
                      xValue: Lib,
                      yValues: Qte)
                    // xValue: listeproduit, xField: "Article.Libelle_article",
                    // yValues: listeproduit, yFields: "Quantite")
                    // xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                    //   yValues: new[] { "20", "20", "40", "10", "10" })
           .GetBytes("png");

                return File(Mychart, "image/png");

            }

        }

    }
}
