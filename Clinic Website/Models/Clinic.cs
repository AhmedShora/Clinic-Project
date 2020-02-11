using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinic_Website.Models
{
    public class Clinic
    {
        public int Id { get; set; }
        [DisplayName("Clinic Name")]
        public string ClinicName { get; set; }
        [DisplayName("Clinic Description")]
        [AllowHtml]//to allow string to be a html code
        public string ClinicDescription { get; set; }
        [DisplayName("Clinic Image")]
        public string ClinicImage { get; set; }

        //This is one to many relationship
        [DisplayName("Clinic Category")]
        public int CategoryId { get; set; }
        //the Doctor id
        public string UserId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}