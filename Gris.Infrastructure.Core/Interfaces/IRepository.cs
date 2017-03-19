using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Gris.Infrastructure.Core.Interfaces
{
    /// <summary>
    /// Generic Repository Interface
    /// </summary>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        ///   Get the total entities count.
        /// </summary>
        int Count { get; }

        /// <summary>
        ///   Gets all entities from database.
        /// </summary>
        /// <returns>Entities as IEnumerable.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        ///   Get entity by identifier.
        /// </summary>
        /// <param name="id">Specified an identifier.</param>
        /// <returns>Entity</returns>
        T GetById(object id);

        /// <summary>
        ///   Get one enitty or default by filter function.
        /// </summary>
        /// <param name="filter">A function to filter returned entities.</param>
        /// <param name="includes">A lambda expression representing the path(s) to include</param>
        /// <returns>Entity</returns>
        T OneOrDefault(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);

        /// <summary>
        ///   Gets entities via optional filter, sort order, and includes.
        /// </summary>
        /// <param name="filter">A function to filter returned entities.</param>
        /// <param name="orderBy">A function to order entities.</param>
        /// <param name="includes">A lambda expression representing the path(s) to include</param>
        /// <returns>Entities as IEnumerable.</returns>
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IEnumerable<T>, IOrderedEnumerable<T>> orderBy = null, params Expression<Func<T, object>>[] includes);

        /// <summary>
        ///   Gets entities from database with filtering and paging.
        /// </summary>
        /// <param name="filter"> A function to filter returned entities.</param>
        /// <param name="total"> Specified the total records count. </param>
        /// <param name="index"> Specified the page index. </param>
        /// <param name="size"> Specified the page size </param>
        IEnumerable<T> FilterWithPaging(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50);

        /// <summary>
        ///   Check if there any entities with the specified filtering criteria.
        /// </summary>
        /// <param name="predicate"> Specified the conditioning filter function. </param>
        bool Contains(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">Entity to add</param>
        /// <returns></returns>
        void Add(T entity);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity to update</param>
        /// <returns></returns>
        void Update(T entity);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        /// <returns></returns>
        void Delete(T entity);

        /// <summary>
        /// Deletes entity by identifier.
        /// </summary>
        /// <param name="id">identifier.</param>
        /// <returns></returns>
        void Delete(object id);
    }
}