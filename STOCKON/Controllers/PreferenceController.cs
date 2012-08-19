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
    public class PreferenceController : Controller
    {
        private STOCKONEntities db = new STOCKONEntities();

        //
        // GET: /Preference/

        public ViewResult Index()
        {
            return View(db.Preference.ToList());
        }


        [GridAction]
        public ActionResult IndexAjax(short id)
        {
            return View(new GridModel(db.Client.ToList()));
        }

        [HttpPost]
        [GridAction]
        public ActionResult Insert(short id)
        {
            //Create a new instance of the EditableCustomer class.
            Preference client = new Preference();

            //Perform model binding (fill the customer properties and validate it).
            if (TryUpdateModel(client))
            {
                //The model is valid - insert the customer.
                client.Id = id;
                db.Preference.Add(client);
                db.SaveChanges();
            }

            //Rebind the grid
            return View(new GridModel(db.Preference.ToList()));
        }

        //
        // GET: /Preference/Details/5

        public ViewResult Details(string id)
        {
            Preference preference = db.Preference.Find(id);
            return View(preference);
        }

        //
        // GET: /Preference/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Preference/Create

        [HttpPost]
        public ActionResult Create(Preference preference)
        {
            if (ModelState.IsValid)
            {
                db.Preference.Add(preference);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(preference);
        }
        
        //
        // GET: /Preference/Edit/5
 
        public ActionResult Edit(short id)
        {
            Preference preference = db.Preference.Find(id);
            return View(preference);
        }

        //
        // POST: /Preference/Edit/5

        [HttpPost]
        public ActionResult Edit(Preference preference, string Categorie, string Remise)
        {
            if (ModelState.IsValid)
            {
                preference.CRecherche = Categorie;
                preference.Remise = Remise;
                db.Entry(preference).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = preference.Id });
            }
            return View(preference);
        }

        //
        // GET: /Preference/Delete/5
 
        public ActionResult Delete(short id)
        {
            Preference preference = db.Preference.Find(id);
            return View(preference);
        }

        //
        // POST: /Preference/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {            
            Preference preference = db.Preference.Find(id);
            db.Preference.Remove(preference);
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