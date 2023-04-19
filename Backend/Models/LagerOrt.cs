using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class LagerOrt : BasisEntitaet
    {
        public int Id { get; set; }
        public string LagerName { get; set; }
        public int VerweildauerGelb { get; set; }
        public int VerweildauerRot { get; set; }

    }
}
