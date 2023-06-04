using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Models
{
    public class Diagnose : BaseEntity
    {

        [Column(TypeName = "nvarchar(650)")]
        public string Name
        {
            get; set;
        }
        public List<Pcb> Pcbs
        {
            get; set;
        }

        // https://stackoverflow.com/questions/34140431/how-do-you-bind-a-comboboxs-selecteditem-to-an-object-that-is-a-copy-of-an-item
        // Override Equals and GetHashCode to compare two StorageLocations
        // needed in Pcb update page to be able to display the current Diagnose of the Pcb
        // Selected Diagnose(when already exists) is diffrent object compared to itemsource of Combobox(Diagnoses)
        // They need to be equal to each other so selecteditem can get set when navigated to.
        public override bool Equals(object obj)
        {
            Diagnose diagnose = obj as Diagnose;
            return diagnose.Id == Id && diagnose.Name == Name;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() & Name.GetHashCode();
        }

    }
}
