
namespace App.Core.Models.Enums
{
    // Das Enum "StorageLocationFilterOptions" definiert verschiedene Optionen für die Filterung von Speicherorten
    public enum StorageLocationFilterOptions
    {
        DwellTimeYellowHigh,  // Option: Hohe Verweildauer (Gelb)
        DwellTimeYellowLow,   // Option: Niedrige Verweildauer (Gelb)
        DwellTimeRedHigh,     // Option: Hohe Verweildauer (Rot)
        DwellTimeRedLow,      // Option: Niedrige Verweildauer (Rot)
        None,                 // Option: Keine Filterung
        Search                // Option: Suche
    }
}
