using App.Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Helpers
{
    public static class ConnectionHelper
    {
        public static async Task<bool> CanConnect(BoschContext boschContext)
        {
            try
            {
                return await boschContext.Database.CanConnectAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
