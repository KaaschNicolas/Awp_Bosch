using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.DTOs
{
    [Keyless]
    public class DashboardStorageLocationDTO
    {
        public string StorageName { get; set; }
        public int CountPcbs { get; set; }
        public int CountGreen { get; set; }
        public int CountYellow { get; set; }
        public int CountRed { get; set; }

        [NotMapped]
        public int Percentage { get; set; }

        [NotMapped]
        public string Number { get; set; }
    }
}
