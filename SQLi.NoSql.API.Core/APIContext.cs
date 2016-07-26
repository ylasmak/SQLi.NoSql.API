using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLi.NoSql.API.Core
{
  public   class APIContext
    {
        private static APIContext _instance;

        public static APIContext Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new APIContext();

                return _instance;
            }
        }


        public string DbContext { get; set; }
        public string ConnexionString { get; set; }
    }
}
