using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entityframework_MSSQL_Test.Models
{
    public class Email
    {

        public int  Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string EmailAddress { get; set; }
    }
}
