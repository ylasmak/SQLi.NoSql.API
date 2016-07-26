using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cassandra.Mapping.Attributes;


namespace SQLi.NoSql.API.Collection
{
   
    [Table(AllowFiltering = true , Name ="User")]
    public class User1
    {
        [PartitionKey]
        public Guid UserId { get; set; }
        [SecondaryIndex]
        public string Name { get; set; }
       [SecondaryIndex]
        public string Group { get; set; }
    }
}
