using System;
using System.Linq;
using System.Linq.Expressions;
namespace SQLI.FWK.Repository
{
    public interface IRepository<T> where T : class, new()
    {

       void DisposeContext();

       object Context { get; }

       void SaveChanges();

        /// <summary>
        /// Ajoute une entité du type TEntity
        /// </summary>
        /// <param name="entity">Entité</param>
       void Add<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Met à jour une entité du type TEntity
        /// </summary>
        /// <param name="entity">Entité</param>
       void Update<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Supprime une entité du type TEntity
        /// </summary>
        /// <param name="entity">Entité</param>
       void Delete<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Supprime un ensemble d'entités correspondant à un type TEntity
        /// </summary>
        /// <param name="where">Condition de recherche des entités</param>
       void Delete<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class;

       

        /// <summary>
        /// Renvoie l'ensemble des entitiés du type TEntity
        /// </summary>
        /// <param name="where">Filtre, doit représenter une condtion unique sinon une exception sera levé</param>
        /// <param name="includes">Includes à appliquer</param>
        /// <returns>Entité correspondant au filtre</returns>
       IQueryable<TEntity> GetListEntities<TEntity>(Expression<Func<TEntity, Boolean>> where, params Expression<Func<TEntity, object>>[] includes) where TEntity : class;

        /// <summary>
        /// Renvoie l'ensemble des entitiés du type TEntity
        /// </summary>
        /// <param name="includes">Includes à appliquer</param>
        /// <returns>L'ensemble des entitiés du type TEntity</returns>
        IQueryable<TEntity> GetListEntities<TEntity>(params Expression<Func<TEntity, object>>[] includes) where TEntity : class;

       
    }
}
