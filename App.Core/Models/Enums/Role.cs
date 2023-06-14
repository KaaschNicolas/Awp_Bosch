using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models.Enums
{
    public enum Role
    {
        [Description("Lesezugriff")]
        Lesezugriff,
        [Description("Admin")]
        Admin,
        [Description("Schichtleitung")]
        Schichtleitung
    }
}
