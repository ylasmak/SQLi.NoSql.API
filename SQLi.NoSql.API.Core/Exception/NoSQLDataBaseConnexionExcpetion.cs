using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLi.NoSql.API.Core.Exception
{
   public class NoSQLDataBaseConnexionExcpetion : System.Exception
    {
        public  NoSQLDataBaseConnexionExcpetion(string message):base(message)
        {
        }
    }
}
