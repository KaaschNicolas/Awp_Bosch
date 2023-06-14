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
    public class EvaluationStorageLocationDTO : BaseEntity
    {
        public int Id { get; set; }

        public string StorageName { get; set; }

        public List<Pcb> Pcbs { get; set; }

        public int PcbCount { get; set; }

        public int BeforeDate { get; set; }

        public int AfterDate { get; set; }
    }
}
