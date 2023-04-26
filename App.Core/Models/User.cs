using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class User : BaseEntity
    {
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string Name
        {
            get; set;
        }
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string AdUsername
        {
            get; set;
        }
        public List<Transfer> Umbuchungen
        {
            get; set;
        }
        public List<Comment> Anmerkungen
        {
            get; set;
        }
    }
}
