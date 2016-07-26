using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SQLi.NoSql.API.Core;
using System.IO;

namespace SQLi.NoSql.API.MongoDB
{
    public class MongoDBRepository<T> : SQLI.FWK.Repository.IRepository<T>
        where T : class, new()
    {

        #region Reference Manager
        private IMongoDatabase database;
        public object Context
        {
            get
            {
                if (database == null)
                {
                    database = (new T() as MongoDBContext).CreateDataBase() as IMongoDatabase;
                }
                return database;
            }
        }

        public IMongoDatabase MongoContext
        {
            get
            {
                return (this.Context as IMongoDatabase);
            }
        }



        public void DisposeContext()
        {
            database = null;
        }

        #endregion

        #region Add methods
        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            var bson = entity.ToBsonDocument();
            var collection = GetMongoCollection(entity);
            collection.InsertOne(bson);         
        }
        
        public Task AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            var bson = entity.ToBsonDocument();
            var collection = GetMongoCollection(entity);
            return collection.InsertOneAsync(bson);
        }

        public  void AddMany<TEntity>(IEnumerable< TEntity> entity) where TEntity : class
        {
            var bson = ToArrayBsonDocument(entity);
            var collection = GetMongoCollection(entity);
            collection.InsertMany(bson);
        }

        public Task AddManyAsync<TEntity>(IEnumerable<TEntity> entity) where TEntity : class
        {
            var bson = ToArrayBsonDocument(entity);
            var collection = GetMongoCollection(entity);
            return collection.InsertManyAsync(bson);
        }

        public Task AddOneAsync(string collectionName, string jsonDocument)
        {
            var collection = GetMongoCollection("restaurants");
            return  collection.InsertOneAsync(jsonDocument.ToBsonDocument());

        }


        public void AddOne(string collectionName, BsonDocument document)
        {
            var collection = GetMongoCollection(collectionName);
            
            collection.InsertOne(document);

        }

        public IFindFluent<BsonDocument, BsonDocument> FinObjectByKey(string collectionName , string key)
        {
            var collection = GetMongoCollection(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", key);
            var result = collection.Find(filter);
            return result;

        }

        #endregion

        #region Delete methods
        public void Delete<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            var collectionName = typeof(TEntity).Name;
            var collection = MongoContext.GetCollection<TEntity>(collectionName);
            var result = collection.DeleteMany(where);
        }
        
        public Task<DeleteResult> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            var collectionName = typeof(TEntity).Name;
            var collection = MongoContext.GetCollection<TEntity>(collectionName);
            var result = collection.DeleteManyAsync(where);
            return result;
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            var collectionName = typeof(TEntity).Name;
            var collection = MongoContext.GetCollection<BsonDocument>(collectionName);
            var res = collection.DeleteOne(entity.ToBsonDocument());

        }

        public Task<DeleteResult> DeleteAsynck<TEntity>(TEntity entity) where TEntity : class
        {
            var collectionName = typeof(TEntity).Name;
            var collection = MongoContext.GetCollection<BsonDocument>(collectionName);
            var res = collection.DeleteOneAsync(entity.ToBsonDocument());
            return res;

        }
        #endregion
               
        #region queryable methods

        public IQueryable<TEntity> GetListEntities<TEntity>(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {

            var collectionName = typeof(TEntity).Name;
            var collection = MongoContext.GetCollection<TEntity>(collectionName);
            var res = collection.AsQueryable<TEntity>().Where(where);
            return res;
        }

        public IQueryable<TResult> GetListEntities<TEntity, TResult>(Expression<Func<TEntity, TResult>> selector, params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {

            var collectionName = typeof(TEntity).Name;
            var collection = MongoContext.GetCollection<TEntity>(collectionName);
            var res = collection.AsQueryable<TEntity>().Select(selector);
            return res;
        }

        public IQueryable<TEntity> GetListEntities<TEntity>(params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            throw new NotImplementedException(ExcpetionMessages.MongoDBNotSuppportIncludeEntities);
        }

        #endregion      

        #region Replace methods
        public void ReplaceOne<TEntity>(TEntity entity) where TEntity : MongoDBCollectionBase
        {
            var collectionName = typeof(TEntity).Name;
            var collection = MongoContext.GetCollection<TEntity>(collectionName);
            collection.ReplaceOne<TEntity>(p => p._id == entity._id, entity);

        }


        public void ReplaceOne<TEntity>(string key ,string collectionName, TEntity entity) 
        {
    
            var collection = MongoContext.GetCollection<TEntity>(collectionName);
            var filter = Builders<TEntity>.Filter.Eq("_id", key);
            collection.ReplaceOne(filter, entity);

        }

        public Task<ReplaceOneResult> ReplaceAsync<TEntity>(TEntity entity) where TEntity : MongoDBCollectionBase
        {
            var collectionName = typeof(TEntity).Name;
            var collection = MongoContext.GetCollection<TEntity>(collectionName);
            return collection.ReplaceOneAsync<TEntity>(p => p._id == entity._id, entity);

        }
        #endregion

        #region Update methods

        public void UpdateMany<TEntity>(Expression<Func<TEntity, bool>> filter, MongoDBUpdateDefinition<TEntity> update) where TEntity : class
        {
            var collectionName = typeof(TEntity).Name;
            var collection = MongoContext.GetCollection<TEntity>(collectionName);
            collection.UpdateMany(filter, update.Build());

        }

        public Task<UpdateResult> UpdateManyAsyn<TEntity>(Expression<Func<TEntity, bool>> filter, MongoDBUpdateDefinition<TEntity> update) where TEntity : class
        {
            var collectionName = typeof(TEntity).Name;
            var collection = MongoContext.GetCollection<TEntity>(collectionName);
            return collection.UpdateManyAsync(filter, update.Build());

        }
        #endregion
        
        #region Import methods

        public void Import(string filePath, string collectionName)
        {
            //open Json Files
            var lines = File.ReadAllLines(filePath);
            var bsonDocumentList = new List<BsonDocument>();
            foreach (var line in lines)
            {
                var bson = BsonDocument.Parse(line);
                bsonDocumentList.Add(bson);
            }

            var collection = GetMongoCollection(collectionName);
            collection.InsertMany(bsonDocumentList);

        }
        #endregion

        #region private methods
        private IMongoCollection<BsonDocument> GetMongoCollection(object entity)
        {
            return MongoContext.GetCollection<BsonDocument>(entity.GetType().Name);
        }

        private IMongoCollection<BsonDocument> GetMongoCollection(string collectionName)
        {
            return MongoContext.GetCollection<BsonDocument>(collectionName);
        }


        private IEnumerable<BsonDocument> ToArrayBsonDocument<TEntity>(IEnumerable<TEntity> source)where TEntity:class
        {
            var list = new List<BsonDocument>();

            if (source != null)

                foreach (var item in source)
                {
                    list.Add(item.ToBsonDocument());
                }

            return list;
        }
        #endregion
        
        #region Not implemented methods
        public void SaveChanges()
        {
            throw new NotImplementedException(ExcpetionMessages.MongoDBNotSuppportSaveChanges);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            throw new NotImplementedException(ExcpetionMessages.MongoDBNotSuppportUpdateUseReplaceOneMethod);

        }
        #endregion

    }
}
