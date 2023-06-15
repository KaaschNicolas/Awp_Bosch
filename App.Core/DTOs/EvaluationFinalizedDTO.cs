using App.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.DTOs
{
    [Keyless]
    public class EvaluationFinalizedDTO
    {
        public int TotalInProgress { get; set; }

        public int TotalFinalized { get; set; }
    }
}
