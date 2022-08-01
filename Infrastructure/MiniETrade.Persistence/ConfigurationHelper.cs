using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Persistence
{
    internal static class ConfigurationHelper
    {
        public static string ConnectionString
        {
            get
            {
                ConfigurationManager cm = new();
                cm.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/MiniETrade.API"));
                cm.AddJsonFile("appsettings.json");
                return cm.GetConnectionString("PostgreSQL");
            }
        }
    }
}
