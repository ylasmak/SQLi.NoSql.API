using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using SQLi.NoSql.API.Core;

using Cassandra;
using Cassandra.Data;
using Cassandra.Data.Linq;
using SQLi.NoSql.API.Collection;

namespace SQLi.NoSql.API.Cassandra
{
    public class CassandraRepository<T> : SQLI.FWK.Repository.IRepository<T>
          where T : class, new()
    {

        ISession _database;
        
        public object Context
        {
            get
            {
                if (_database == null)
                {
                    _database =(new T() as IDataBaseContext).CreateDataBase() as ISession;
                }

                return _database;
            }
        }

        public ISession CassandraContext
        {
            get
            {
                return (this.Context as ISession);
            }
        }
       public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            var table = new Table<TEntity>(CassandraContext);
            table.CreateIfNotExists();
            var query = table.Insert(entity);
            query.Execute();


          
        }

        public void Delete<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class
        {

           var table = new Table<TEntity>(CassandraContext);
            var query = table.DeleteIf(where);
           query.Execute();
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            //var table = new Table<TEntity>(CassandraContext);
            //var query = table.
            //query..Execute();
        }

        public void DisposeContext()
        {
            CassandraContext.Dispose();
        }

        public IQueryable<TEntity> GetListEntities<TEntity>(params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            throw new Exception(ExcpetionMessages.CassandraNotSuppportIncludeEntities);
        }

        public IQueryable<TEntity> GetListEntities<TEntity>(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            var table = new Table<TEntity>(CassandraContext);
            var query = table.Where(where);
            return query.Execute().AsQueryable();
        }

        public void SaveChanges()
        {
            throw new Exception(ExcpetionMessages.CassandraNotSuppportSaveChanges);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            throw new Exception(ExcpetionMessages.CassandraNotSuppportSaveChanges);
        }

        public void Update<TEntity>(TEntity entity ,Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            var table = new Table<TEntity>(CassandraContext);
            var query = table.Where(where).Select(p => entity).Update();
            query.Execute();
        }
    }
}
