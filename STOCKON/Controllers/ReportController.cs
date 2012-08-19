using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.IO;
using ReportManagement;
using STOCKON.Models;

namespace STOCKON.Controllers
{
   

    public class ReportController : PdfViewController
    {
        //
        // GET: /Report/
        private STOCKONEntities db = new STOCKONEntities();

        public ActionResult Index()
        {
            return View();
        }

        /*public ActionResult ReportClient()
        {

            ReportClass rptH = new ReportClass();
            

              //  rptH.FileName = Server.MapPath("~/") + "//Report//listeclient.rpt";
                string path = Server.MapPath("~/Report/listeclient.rpt");
                rptH.FileName = path;
                rptH.Load();
              //  rptH.Refresh();
                Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
                //rptH.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                  
        
        }

        public void ReportFournisseur()
        {

            using (ReportClass rptH = new ReportClass())
            {

                rptH.FileName = Server.MapPath("~/") + "//Report//listefournisseur.rpt";
                rptH.Load();
                rptH.Refresh();
                rptH.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
            }

        }

        public void ReportEtatstock()
        {

            using (ReportClass rptH = new ReportClass())
            {

                rptH.FileName = Server.MapPath("~/") + "//Report//etatdustock.rpt";
                rptH.Load();
                rptH.Refresh();
                rptH.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
            }

        }

        public void FactureV(short id)
        {
            int i;
            i = int.Parse(id.ToString());
            using (ReportClass rptH = new ReportClass())
            {
                

                rptH.FileName = Server.MapPath("~/") + "//Report//facture.rpt";
                rptH.Load();
                rptH.Refresh();
                rptH.SetParameterValue("@id", i);
                rptH.SetParameterValue("@id", i, "Entetefacture");
               
                rptH.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
            }

        }*/

        public ActionResult FactureVente(short id)
        {
            return this.ViewPdf("", "FactureVente", Createfacture(id));
        }

        private ModelFacture Createfacture(short id)
        {
            ModelFacture modelfacture = new ModelFacture();
            Facture_vente Fa = new Facture_vente();
            Preference preference = new Preference();

              var facture = from l in db.Facture_vente.Include(l => l.Client)
                                where l.Id_facture == id
                                select l;

              var ListeP = from l in db.Liste_produit_vente.Include(l => l.Article)
                            where l.Id_facture == id
                            select l;

              preference = db.Preference.Find(1);

              Fa = facture.ToList().ElementAt(0);
              if (Fa.Type_facture == "Facture" || Fa.Type_facture == "Facture comptabilisée")
                  modelfacture.Type_Op = "Facture de vente";
              else
                  modelfacture.Type_Op = Fa.Type_facture;

              modelfacture.Remise = (double)Fa.Remise;
              modelfacture.TRemise = preference.Remise;
              modelfacture.TVA = (Boolean)Fa.TVA;
              modelfacture.Nom_client = Fa.Client.Nom_client + " " + Fa.Client.Prenom_client;
              modelfacture.Telephone_client = Fa.Client.Telephone_client;
              modelfacture.Adresse_client = Fa.Client.Adresse_client;
              modelfacture.Num_Facture = Fa.Code_facture;
              modelfacture.Date_Facture = Fa.Date_facture.ToShortDateString();
              modelfacture.Nom_E = preference.Nom;
              modelfacture.Adresse_E = preference.Adresse;
              modelfacture.Numtel_E = preference.Num_telephone;
              modelfacture.Liste_produit_vente = ListeP.ToList();
              modelfacture.TauxTva = (double)preference.Taux_tva;  

            return modelfacture;

        }

        public ActionResult FactureAchat(short id)
        {
            return this.ViewPdf("", "FactureAchat", CreatefactureA(id));
        }

        private ModelFacture CreatefactureA(short id)
        {
            ModelFacture modelfacture = new ModelFacture();
            Facture_achat Fa = new Facture_achat();
            Preference preference = new Preference();

            var facture = from l in db.Facture_achat.Include(l => l.Fournisseur)
                          where l.Id_facture == id
                          select l;

            var ListeP = from l in db.Liste_produit_achat.Include(l => l.Article)
                         where l.Id_facture == id
                         select l;

            preference = db.Preference.Find(1);

            Fa = facture.ToList().ElementAt(0);

            if (Fa.Type_facture == "Facture" || Fa.Type_facture == "Facture comptabilisée")
                modelfacture.Type_Op = "Facture de vente";
            else
                modelfacture.Type_Op = Fa.Type_facture;

            modelfacture.Nom_client = Fa.Fournisseur.Nom_fournisseur + " " + Fa.Fournisseur.Prenom_fournisseur;
            modelfacture.Telephone_client = Fa.Fournisseur.Telephone_fournisseur;
            modelfacture.Adresse_client = Fa.Fournisseur.Adresse_fournisseur;
            modelfacture.Num_Facture = Fa.Code_facture;
            modelfacture.Date_Facture = Fa.Date_facture.ToShortDateString();
            modelfacture.Nom_E = preference.Nom;
            modelfacture.Adresse_E = preference.Adresse;
            modelfacture.Numtel_E = preference.Num_telephone;
            modelfacture.Liste_produit_achat = ListeP.ToList();
            

            return modelfacture;

        }

        public ActionResult MouvementS(short id)
        {
            return this.ViewPdf("", "MouvementS", CreatefactureM(id));
        }

        private ModelFacture CreatefactureM(short id)
        {
            ModelFacture modelfacture = new ModelFacture();
            Mouvement_stock Ms = new Mouvement_stock();
            Preference preference = new Preference();

            var facture = from l in db.Mouvement_stock
                          where l.ID_mouvement == id
                          select l;

            var ListeP = from l in db.Liste_produit.Include(l => l.Article)
                         where l.Id_operation == id
                         select l;

            preference = db.Preference.Find(1);

            Ms = facture.ToList().ElementAt(0);



            modelfacture.Num_Facture = Ms.Code_mouvement;
            modelfacture.Type_Op = Ms.Type_mouvement;
            modelfacture.Date_Facture = Ms.Date_mouvement.ToShortDateString();
            modelfacture.Nom_E = preference.Nom;
            modelfacture.Adresse_E = preference.Adresse;
            modelfacture.Numtel_E = preference.Num_telephone;
            modelfacture.Liste_produit = ListeP.ToList();


            return modelfacture;

        }

         public ActionResult EtatClient()
        {
            return this.ViewPdf("Liste des clients", "EtatClient", CreateListClient());
        }

         private List<Client> CreateListClient()
         {
             var clients = from c in db.Client
                           orderby c.Nom_client ascending
                           select c;
             return clients.ToList();
         }

         public ActionResult EtatFournisseur()
         {
             return this.ViewPdf("Liste des fournisseurs", "EtatFournisseur", CreateListFournisseur());
         }

         private List<Fournisseur> CreateListFournisseur()
         {
             var fournisseurs = from c in db.Fournisseur
                                 orderby c.Nom_fournisseur ascending
                                select c;
             return fournisseurs.ToList();

         }

         public ActionResult EtatduStock()
         {
             return this.ViewPdf("Etat du stock", "EtatduStock", CreateListStock());
         }

         private List<Article> CreateListStock()
         {
             var article = from c in db.Article.Include(c => c.Famille_article)
                           orderby c.Libelle_article ascending
                                select c;
             return article.ToList();

         }

         public ActionResult EtatVente(DateTime DateD, DateTime DateF)
         {
             return this.ViewPdf("Ventes du " + DateD.ToShortDateString() + " Au " + DateF.ToShortDateString(), "EtatVente", CreateListVente(DateD, DateF));
         }

         private List<Facture_vente> CreateListVente(DateTime DateD, DateTime DateF)
         {
             
           
             var facture_vente = from c in db.Facture_vente.Include(c => c.Client).Include(c => c.Liste_produit_vente)
                                 where c.Date_facture >= DateD && c.Date_facture <= DateF
                                 orderby c.Date_facture descending
                           select c ;
             return facture_vente.ToList();

         }

         public ActionResult EtatAchat(DateTime DateD, DateTime DateF)
         {
             return this.ViewPdf("Achats du " + DateD.ToShortDateString() + " Au " + DateF.ToShortDateString(), "EtatAchat", CreateListAchat(DateD, DateF));
         }

         private List<Facture_achat> CreateListAchat(DateTime DateD, DateTime DateF)
         {


             var facture_achat = from c in db.Facture_achat.Include(c => c.Fournisseur).Include(c => c.Liste_produit_achat)
                                 where c.Date_facture >= DateD && c.Date_facture <= DateF
                                 orderby c.Date_facture descending
                                 select c;
             return facture_achat.ToList();

         }

         public ActionResult EtatEntreStock(DateTime DateD, DateTime DateF)
         {
             return this.ViewPdf("Mouvement d'entrée en stock du " + DateD.ToShortDateString() + " au " + DateF.ToShortDateString(), "EtatEntreStock", CreateListEntre(DateD, DateF));
         }

         private List<Mouvement_stock> CreateListEntre(DateTime DateD, DateTime DateF)
         {


             var mouvement_stock = from c in db.Mouvement_stock.Include(c => c.Liste_produit)
                                   where c.Date_mouvement >= DateD && c.Date_mouvement <= DateF && c.Type_mouvement == "Entrée en stock"
                                 orderby c.Date_mouvement descending
                                 select c;
             return mouvement_stock.ToList();

         }

         public ActionResult EtatSortieStock(DateTime DateD, DateTime DateF)
         {
             return this.ViewPdf("Mouvement de sortie en stock du " + DateD.ToShortDateString() + " au " + DateF.ToShortDateString(), "EtatSortieStock", CreateListSortie(DateD, DateF));
         }

         private List<Mouvement_stock> CreateListSortie(DateTime DateD, DateTime DateF)
         {


             var mouvement_stock = from c in db.Mouvement_stock.Include(c => c.Liste_produit)
                                   where c.Date_mouvement >= DateD && c.Date_mouvement <= DateF && c.Type_mouvement != "Entrée en stock"
                                   orderby c.Date_mouvement descending
                                   select c;
             return mouvement_stock.ToList();

         }

         public ViewResult EtatP(string id)
         {

             return View();
         }

       [HttpPost]
         public ActionResult EtatP(string id, DateTime DateD, DateTime DateF)
         {
           switch(id) 
           { 
               case "fv" :
                 return RedirectToAction("EtatVente","Report",new {DateD = DateD, DateF = DateF});
                     // break;
               case "fa" :
                 return RedirectToAction("EtatAchat", "Report", new { DateD = DateD, DateF = DateF });
                      //break;
               case "me":
                 return RedirectToAction("EtatEntreStock", "Report", new { DateD = DateD, DateF = DateF });
               case "ms":
                 return RedirectToAction("EtatSortieStock", "Report", new { DateD = DateD, DateF = DateF });

         

           }
             return View();
         }

    /*    public void FactureA(short id)
        {

            using (ReportClass rptH = new ReportClass())
            {

               
                rptH.FileName = Server.MapPath("~/") + "//Report//facturea.rpt";
                rptH.Load();
                rptH.Refresh();
                rptH.SetParameterValue("@id", id);
                rptH.SetParameterValue("@id", id, "Entetefacture");
                rptH.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
            }

        }*/



    }
}
