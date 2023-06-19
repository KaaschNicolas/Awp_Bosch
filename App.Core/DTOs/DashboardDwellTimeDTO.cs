using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.DTOs
{
    [Keyless]
    public class DashboardDwellTimeDTO
    {
        public int DwellTimeStatus { get; set; }
        public int CountDwellTimeStatus { get; set; }

        [NotMapped]
        public string Color { get; set; }

        [NotMapped]
        public int Percentage { get; set; }
    }
}
