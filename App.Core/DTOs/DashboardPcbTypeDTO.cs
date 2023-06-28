using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.DTOs
{
    public class DashboardPcbTypeDTO
    {
        public int Count { get; set; }
        public string PcbPartNumber { get; set; }

        [NotMapped]
        public string Number { get; set; }

        [NotMapped]
        public int Percentage { get; set; }
    }
}
