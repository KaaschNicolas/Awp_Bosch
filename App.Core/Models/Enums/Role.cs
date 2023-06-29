using System.ComponentModel;

namespace App.Core.Models.Enums
{
    // Das Enum "Role" definiert verschiedene Benutzerrollen (Berechtigungen)
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
