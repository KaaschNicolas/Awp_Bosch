using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public abstract class BasisEntitaet
    {
        public DateTime CreatedDate
        {
            get; set;
        }
        public DateTime DeletedDate
        {
            get; set;
        }
    }
}
