using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entityframework_MSSQL_Test.Models
{
    public class Address
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string City { get; set; }
        
        [Required]
        public string Street { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string HouseNumber { get; set; }

        [Required]
        [MaxLength(10)]
        [Column(TypeName = "varchar(10)")]
        public string PostalCode { get; set; }

        [Required]
        [MaxLength(200)]
        public string Country { get; set; }
    }
}
