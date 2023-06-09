﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Models
{
    // Die Klasse "StorageLocation" erbt von der Basisklasse "BaseEntity" (Lagerorte)
    public class StorageLocation : BaseEntity
    {
        // Der Name des Lagerorts
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string StorageName
        {
            get; set;
        }
        // Die Zeit der Verweildauer des Lagerorts, bis die Gelbphase erreicht wurde
        public string DwellTimeYellow
        {
            get; set;
        }
        // Die Zeit der Verweildauer des Lagerorts, bis die Rotphase erreicht wurde
        public string DwellTimeRed
        {
            get; set;
        }
        // Gibt an, ob der Lagerort ein endgültiger Lagerort ist oder nicht
        public bool IsFinalDestination
        {
            get; set;
        }
        // Eine Liste von Umbuchungen, die diesem Lagerort zugeordnet sind
        public List<Transfer> Transfers
        {
            get; set;
        }


        // https://stackoverflow.com/questions/34140431/how-do-you-bind-a-comboboxs-selecteditem-to-an-object-that-is-a-copy-of-an-item
        // Override Equals and GetHashCode to compare two StorageLocations
        // needed in Pcb update page to be able to display the current StorageLocation of the transfers
        // item source of Itemscontrol (Transfers.StorageLocations) is diffrent object compared to itemsource of Combobox(StorageLocations)
        // They need to be equal to each other so selecteditem can get set when navigated to.
        public override bool Equals(object obj)
        {
            StorageLocation storageLocation = obj as StorageLocation;
            return storageLocation.Id == Id && storageLocation.StorageName == StorageName;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() & StorageName.GetHashCode();
        }

    }
}
