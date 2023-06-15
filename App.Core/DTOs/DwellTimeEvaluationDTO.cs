using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.DTOs
{
    [Keyless]
    public class DwellTimeEvaluationDTO
    {
        public string StorageName { get; set; }
        public double AvgDwellTime { get; set; }
    }
}
