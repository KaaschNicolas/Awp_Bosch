using App.Core.Models;
using App.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    class EvaluationLocation
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Pcb> Pcbs { get; set; }

        public int CountPcbs { get; set; }

        public static EvaluationLocation ToEvaluationLocation(StorageLocation location, List<Pcb> pcbs)
        {
            return new EvaluationLocation
            {
                Id = location.Id,
                Name = location.StorageName,
                Pcbs = pcbs,
                CountPcbs = pcbs.Count,
            };
        }


    }
}
