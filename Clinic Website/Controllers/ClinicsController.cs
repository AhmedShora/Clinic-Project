using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Clinic_Website.Models;
using System.IO;
using Microsoft.AspNet.Identity;

namespace Clinic_Website.Controllers
{
    [Authorize]
    public class ClinicsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var clinics = db.Clinics.Include(j => j.Category);
            return View(clinics.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinic clinic = db.Clinics.Find(id);
            if (clinic == null)
            {
                return HttpNotFound();
            }
            return View(clinic);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Clinic clinic,HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
               
                if (upload != null)
                {
                    //دول تلات أوامر للحفظ
                    string path = Path.Combine(Server.MapPath("~/Uploads"),upload.FileName);
                    upload.SaveAs(path);
                    clinic.ClinicImage = upload.FileName;
                    clinic.UserId = User.Identity.GetUserId();
                    
                }

                var UserId = User.Identity.GetUserId();
                db.Clinics.Add(clinic);
                db.SaveChanges();
                return RedirectToAction("Home","Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", clinic.CategoryId);
            return View(clinic);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinic clinic = db.Clinics.Find(id);
            if (clinic == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", clinic.CategoryId);
            return View(clinic);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Clinic clinic, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string oldPath = Path.Combine(Server.MapPath("~/Uploads"), clinic.ClinicImage);

                if (upload != null)
                {
                    System.IO.File.Delete(oldPath);
                    string path = Path.Combine(Server.MapPath("~/Uploads"), upload.FileName);
                    upload.SaveAs(path);
                    clinic.ClinicImage = upload.FileName;
                }

                db.Entry(clinic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
                
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", clinic.CategoryId);
            return View(clinic);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinic clinic = db.Clinics.Find(id);
            if (clinic == null)
            {
                return HttpNotFound();
            }
            return View(clinic);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clinic clinic = db.Clinics.Find(id);
            db.Clinics.Remove(clinic);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
