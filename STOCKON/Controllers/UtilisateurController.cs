using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STOCKON.Models;
using System.Reflection;
using System.IO;
using System.Text;

namespace STOCKON.Controllers
{ 
    public class UtilisateurController : Controller
    {
        private STOCKONEntities db = new STOCKONEntities();

        //
        // GET: /Utilisateur/

        public ViewResult Index()
        {
            return View(db.Utilisateur.ToList());
        }

        //
        // GET: /Utilisateur/Details/5

        public ViewResult Details(short id)
        {
            Utilisateur utilisateur = db.Utilisateur.Find(id);
            return View(utilisateur);
        }


        public ActionResult LogOn()
        {

            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel logonmodel)
        {
            try
            {

                var utilisateur = from u in db.Utilisateur
                                  where u.Login == logonmodel.Login && u.Pasword == logonmodel.Password
                                  select u;

                if (utilisateur.Count() != 0)
                {
                    Session.Add("uid", utilisateur.First().Id_utilisateur);
                    Session.Add("nom", utilisateur.First().Login);
                    Session.Add("client", utilisateur.First().Client);
                    Session.Add("article", utilisateur.First().Article);
                    var rr = Session["article"];

                    Session.Add("facture", utilisateur.First().Facture);
                    Session.Add("mouvement_stock", utilisateur.First().Mouvement_stock);
                    Session.Add("etats", utilisateur.First().Etats);
                    Session.Add("fournisseur", utilisateur.First().Fournissaeur);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Le login ou le mot de passe est incorrect");

                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    if (exSub is FileNotFoundException)
                    {
                        FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                ModelState.AddModelError("",sb.ToString());
                //Display or log the error based on your application.
            }
            return View(logonmodel);
        
        }

        public ActionResult LogOff()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel changepwd)
        {
            if (ModelState.IsValid)
            {

                var utilisateur = db.Utilisateur.Find(Session["uid"]);
                utilisateur.Pasword = changepwd.NewPassword;
                
                db.Entry(utilisateur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            return View(changepwd);
        }
        //
        // GET: /Utilisateur/Create


        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Utilisateur/Create

        [HttpPost]
        public ActionResult Create(Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                db.Utilisateur.Add(utilisateur);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(utilisateur);
        }
        
        //
        // GET: /Utilisateur/Edit/5
 
        public ActionResult Edit(short id)
        {
            Utilisateur utilisateur = db.Utilisateur.Find(id);
            return View(utilisateur);
        }

        //
        // POST: /Utilisateur/Edit/5

        [HttpPost]
        public ActionResult Edit(Utilisateur utilisateur, string apw)
        {
            Utilisateur U = new Utilisateur();
            U = db.Utilisateur.Find(utilisateur.Id_utilisateur);
            if (U.Pasword.Trim() == apw.Trim())
            {
                if (ModelState.IsValid)
                {
                    using (STOCKONEntities entity = new STOCKONEntities())
                    {
                        entity.Entry(utilisateur).State = EntityState.Modified;
                        entity.SaveChanges();
                        return RedirectToAction("Index");
                    
                    }
                   
                }
                return View(utilisateur);
            }
            else
            {
                ModelState.AddModelError("", "L'ancien mot de passe est incorrect");
                return View(utilisateur);
            
            }
        }

        //
        // GET: /Utilisateur/Delete/5
 
        public ActionResult Delete(short id)
        {
            Utilisateur utilisateur = db.Utilisateur.Find(id);
            return View(utilisateur);
        }

        //
        // POST: /Utilisateur/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {            
            Utilisateur utilisateur = db.Utilisateur.Find(id);
            db.Utilisateur.Remove(utilisateur);
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