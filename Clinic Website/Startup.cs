using Clinic_Website.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Clinic_Website.Startup))]
namespace Clinic_Website
{
    public partial class Startup
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateDefaultRolesAndUsers();
        }
        public void CreateDefaultRolesAndUsers()
        {
            var roleManger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            IdentityRole role = new IdentityRole();
            if (!roleManger.RoleExists("Admins"))
            {
                if (!roleManger.RoleExists("Doctor"))
                {
                    role.Name = "Doctor";
                }
                if (!roleManger.RoleExists("Patient"))
                {
                    role.Name = "Patient";
                    roleManger.Create(role);
                }
                role.Name = "Admins";
                roleManger.Create(role);
                ApplicationUser user = new ApplicationUser();
                user.UserName = "Ahmed";
                user.Email = "ahmedalshora53@gmail.com";

                var Check = userManger.Create(user, "Aaaa.12345");
                if (Check.Succeeded)
                {
                    userManger.AddToRoles(user.Id, "Admins");
                }
            }
           
        }
    }
}
