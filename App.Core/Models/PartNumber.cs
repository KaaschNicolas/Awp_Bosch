using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Models;

namespace App.Core.Models
{
    public class PartNumber
    {
        public string Number
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
        public int MaxTransfer
        {
            get; set;
        }


        public ICollection<SampleOrder> Orders
        {
            get; set;
        }


    }
}
