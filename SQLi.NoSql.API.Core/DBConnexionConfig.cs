using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SQLi.NoSql.API.Core
{
   public class DBConnexionConfig : ConfigurationSection
    {
        [ConfigurationProperty("Host", IsRequired = true)]
        public string Host
        {
            get
            {
                return (string)this["Host"];
            }
            set {
                this["ServerName"] = value;
            }
        }
        [ConfigurationProperty("Port", IsRequired = false)]
        public int? Port
        {
            get
            {
                return (int?)this["Port"];
            }
            set
            {
                this["Port"] = value;
            }
        }
        [ConfigurationProperty("DataBase", IsRequired = true)]
        public string DataBase
        {
            get
            {
                return (string)this["DataBase"];
            }
            set
            {
                this["DataBase"] = value;
            }
        }

        [ConfigurationProperty("UserName", IsRequired = false)]
        public string UserName
        {
            get
            {
                return (string)this["UserName"];
            }
            set
            {
                this["UserName"] = value;
            }
        }

        [ConfigurationProperty("Password", IsRequired = false)]
        public string Password
        {
            get
            {
                return (string)this["Password"];
            }
            set
            {
                this["Password"] = value;
            }
        }

    }
}
