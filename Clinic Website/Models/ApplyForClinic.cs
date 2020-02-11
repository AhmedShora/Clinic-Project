using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic_Website.Models
{
    public class ApplyForClinic
    {
       
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime ApplyDate { get; set; }

        public int ClinicId { get; set; }
        public string UserId { get; set; }

        public virtual Clinic clinic { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}