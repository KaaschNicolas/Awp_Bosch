using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Nutzer : BasisEntitaet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AdUsername { get; set; }
    }
}
