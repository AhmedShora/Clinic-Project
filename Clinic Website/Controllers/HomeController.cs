using Clinic_Website.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext DB = new ApplicationDbContext();
        public ActionResult Index()
        {
           
            return View(DB.Categories.ToList());
        }
        public ActionResult Details(int ClinicId) {
            var clinic = DB.Clinics.Find(ClinicId);
            if (clinic == null) {
                return HttpNotFound();
            }
            Session["ClinicId"] = ClinicId;
            return View(clinic);
        }
        [Authorize]
        public ActionResult Apply()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Apply(string Message)
        {
            var UserId = User.Identity.GetUserId();
            var ClinicId = (int)Session["ClinicId"];

            var Check = DB.ApplyForClinics.Where(a => a.ClinicId == ClinicId && a.UserId == UserId).ToList();
            if (Check.Count < 1)
            {
                var clinic = new ApplyForClinic();
                clinic.ClinicId = ClinicId;
                clinic.UserId = UserId;
                clinic.Message = Message;
                clinic.ApplyDate = DateTime.Now;

                DB.ApplyForClinics.Add(clinic);
                DB.SaveChanges();

                ViewBag.Result = "Well Done, You applied successfully";
                return View();
            }
            else
            {
                ViewBag.Result = "sorry, you applied before!";
                return View();

            }
        }
        [Authorize]
        public ActionResult GetPatientsforClinics()
        {
            var UserID = User.Identity.GetUserId();

            var Clinics = from app in DB.ApplyForClinics
                       join clinic in DB.Clinics
                       on app.ClinicId equals clinic.Id
                       where clinic.User.Id == UserID
                       select app;

            var grouped = from j in Clinics
                          group j by j.clinic.ClinicName
                          into g
                          select new ClinicViewModel
                          {
                              ClinicName = g.Key,
                              Items = g
                          };
            return View(grouped.ToList());

        }
        
        [Authorize]
        public ActionResult DetailsOfClinics(int Id)
        {
            var clinic = DB.ApplyForClinics.Find(Id);
            if (clinic == null)
            {
                return HttpNotFound();
            }
            return View(clinic);
        }

        public ActionResult Edit(int id)
        {
            var clinic = DB.ApplyForClinics.Find(id);
            if (clinic == null)
            {
                return HttpNotFound();
            }
            return View(clinic);
        }

        [HttpPost]
        public ActionResult Edit(ApplyForClinic clinic)
        {
            if (ModelState.IsValid)
            {
                clinic.ApplyDate = DateTime.Now;
                DB.Entry(clinic).State = EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("GetClinicsByUser");
            }
            return View(clinic);
        }

        public ActionResult Delete(int id)
        {
            var clinic = DB.ApplyForClinics.Find(id);
            if (clinic == null)
            {
                return HttpNotFound();
            }
            return View(clinic);
        }

        [HttpPost]
        public ActionResult Delete(ApplyForClinic clinic)
        {
            try
            {
                var myclinic = DB.ApplyForClinics.Find(clinic.Id);
                DB.ApplyForClinics.Remove(myclinic);
                DB.SaveChanges();
                return RedirectToAction("GetClinicsByUser");
            }
            catch
            {
                return View(clinic);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        //new actions
        [Authorize]
        public ActionResult GetDoctorClinic()
        {
            var UserID = User.Identity.GetUserId();
            //query type
            var Clinics = from app in DB.Clinics
                          where app.UserId == UserID
                          select app;
            return View(Clinics.ToList());
        }

        [Authorize]
        public ActionResult GetClinicsByUser()
        {
            var UserId = User.Identity.GetUserId();
            //non query type
            var Clinics = DB.ApplyForClinics.Where(a => a.UserId == UserId);
            return View(Clinics.ToList());
        }

        //separate doctors and patients from users
        [Authorize]
        public ActionResult GetDoctorsByAdmin()
        {
            //   var AllDoctors = DB.Users;
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(DB));
            var role = roleManager.FindByName("Doctor").Users.First();
            var usersInRoleDoctor = DB.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(role.RoleId)).ToList();
            return View(usersInRoleDoctor);
        }
        [Authorize]
        public ActionResult GetPatientsByAdmin()
        {
            //   var AllDoctors = DB.Users;
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(DB));
            var role = roleManager.FindByName("Patient").Users.First();
            var usersInRolePatient = DB.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(role.RoleId)).ToList();
            return View(usersInRolePatient);
        }
        [HttpGet]
        public ActionResult Contact()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactModel contact)
        {
            var mail = new MailMessage();
            //the  person who will send the email
            var loginInfo = new NetworkCredential("ahmedalshora53@gmail.com", "Password");
            mail.From = new MailAddress(contact.Email);
            mail.To.Add(new MailAddress("ahmedalshora53@gmail.com"));
            mail.Subject = contact.Subject;
            mail.IsBodyHtml = true;

            string body = "Sender Name" + contact.Name + "<br>" +
                        "Sender Email" + contact.Email + "<br>" +
                        "Message Title" + contact.Subject + "<br>" +
                        "Message" + contact.Message;
            mail.Body = body;

            var smptClient = new SmtpClient("smtp.gmail.com",587);
            smptClient.EnableSsl = true;
            smptClient.Credentials = loginInfo;
            smptClient.Send(mail);
            return RedirectToAction("Index");

        }
        //searchName is the same name in input type in _MainLayout 
        [HttpPost]
        public ActionResult Search(string searchName)
        {
            var result = DB.Clinics.Where(aa => aa.ClinicName.Contains(searchName)
            || aa.ClinicDescription.Contains(searchName)
            || aa.Category.CategoryName.Contains(searchName)
            ||aa.Category.CategoryDescription.Contains(searchName)).ToList();
            return View(result);
        }
    }
}