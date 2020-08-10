using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Clinic_Website.Models
{
    public enum BloodType
    {
        [Description("A+")]
        A = 1,

        [Description("A-")]
        A_ = 2,

        [Description("B+")]
        B = 3,

        [Description("B-")]
        B_ = 4,

        [Description("O+")]
        O = 5,

        [Description("O-")]
        O_ = 6,

        [Description("AB+")]
        AB = 7,

        [Description("AB-")]
        AB_ = 8,
    }
}