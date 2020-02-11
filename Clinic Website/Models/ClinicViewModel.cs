using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic_Website.Models
{
    public class ClinicViewModel
    {
        public string ClinicName { get; set; }
        public IEnumerable<ApplyForClinic> Items { get; set; }
       
    }
}