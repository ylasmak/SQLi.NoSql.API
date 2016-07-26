using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;




namespace SQLi.NoSql.API.Core
{
    public class MongoDBUpdateDefinition<TDocument>
    {

        UpdateDefinition<TDocument> _def;
        public  MongoDBUpdateDefinition<TDocument> Set<TField>(Expression<Func<TDocument, TField>> field, TField value)
        {

            if (_def == null)
            {
                _def = Builders<TDocument>.Update.Set(field, value);
            }
            else
            {
                _def = _def.Set(field, value);
            }
            return this;

        }

        public UpdateDefinition<TDocument> Build()
        {
            return _def;
        }
    }
}
