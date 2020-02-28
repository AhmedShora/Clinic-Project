using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Clinic_Website.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Clinic_Website.Models
{
    public class ApplicationUser : IdentityUser
    {
       // public string UserType { get; set; }
        public virtual ICollection< Clinic> Clinics { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Clinic_Website.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<Clinic_Website.Models.Clinic> Clinics { get; set; }

        public System.Data.Entity.DbSet<Clinic_Website.Models.ApplyForClinic> ApplyForClinics { get; set; }


        //public System.Data.Entity.DbSet<Clinic_Website.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}