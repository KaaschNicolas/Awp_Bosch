using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entityframework_MSSQL_Test.Models
{
    public class User
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public int Age { get; set; }
        public int Balance { get; set; }

        public List<Address> Addresses { get; set; }
    }
}
