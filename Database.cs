using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticaSoftLeandroRodriguez
{
    public class Database
    {
        public static string connectionString =>
            "Server=localhost\\SQLEXPRESS;Database=pruebademo;Trusted_Connection=True;Encrypt=False;";
    }
}


