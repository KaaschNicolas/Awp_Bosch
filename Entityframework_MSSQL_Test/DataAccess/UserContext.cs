using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entityframework_MSSQL_Test.DataAccess
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions dbOptions) : base(dbOptions) { }
    }
}
