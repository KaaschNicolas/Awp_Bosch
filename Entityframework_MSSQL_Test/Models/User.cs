using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entityframework_MSSQL_Test.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Age { get; set; }
        
        [Required]
        public string Balance { get; set; }

        public List<Address> Addresses { get; set; } = new List<Address>();
        
        public List<Email> Emails { get; set; } = new List<Email>();
    }
}
