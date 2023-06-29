using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.DTOs
{
    [Keyless]
    public class EvaluationPcbTypeI_ODTO
    {
        public string storage { get; set; }

        public int countAtStorage { get; set; }
    }
}
