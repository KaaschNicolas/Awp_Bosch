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
    public class EvaluationStorageLocationDTO 
    {
        public string StorageName { get; set; }

        public int SumCount { get; set; }

        public int CountBefore { get; set; }

        public int CountAfter { get; set; }
    }
}
