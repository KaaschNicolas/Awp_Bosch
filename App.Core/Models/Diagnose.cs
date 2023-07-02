using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Models
{
    // Die Klasse "Diagnose" erbt von der Basisklasse "BaseEntity" (Fehlerkategorie)
    public class Diagnose : BaseEntity
    {

        // Der Name der Diagnose
        [Column(TypeName = "nvarchar(650)")]
        public string Name
        {
            get; set;
        }
        // Eine Liste von PCBs, die dieser Diagnose zugeordnet sind
        public List<Pcb> Pcbs
        {
            get; set;
        }

        // https://stackoverflow.com/questions/34140431/how-do-you-bind-a-comboboxs-selecteditem-to-an-object-that-is-a-copy-of-an-item
        // Override Equals and GetHashCode to compare two Diagnoses
        // needed in Pcb update page to be able to display the current Diagnose of the Pcb
        // Selected Diagnose(when already exists) is diffrent object compared to itemsource of Combobox(Diagnoses)
        // They need to be equal to each other so selecteditem can get set when navigated to.
        public override bool Equals(object obj)
        {
            Diagnose diagnose = obj as Diagnose;
            return diagnose.Id == Id && diagnose.Name == Name;
        }

        // Überschreiben der GetHashCode-Methode, um den Hashcode der Diagnose zu generieren
        public override int GetHashCode()
        {
            return Id.GetHashCode() & Name.GetHashCode();
        }

    }
}
