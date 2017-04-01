using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace Gris.Infrastructure.Core.Repositories
{
    public class SoftDeleteEFRepository<T> : EFRepository<T>, IDisposable where T : class, ISoftDelete
    {
        public SoftDeleteEFRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        #region IRepository Methods

        public override int Count
        {
            get { return _dbSet.Count(t => t.Active); }
        }

        public override IEnumerable<T> GetAll()
        {
            return _dbSet.Where(t => t.Active).AsEnumerable();
        }

        public override T GetById(object id)
        {
            T entity = _dbSet.Find(id);
            if (!entity.Active)
                return null;

            return entity;
        }

        public override T OneOrDefault(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            Func<T, bool> isActiveFunc = (t) => t.Active;
            Expression<Func<T, bool>> filterWithActive = Expression.Lambda<Func<T, bool>>(
                Expression.And(filter.Body, Expression.Call(isActiveFunc.Method)));

            return base.OneOrDefault(filterWithActive, includes);
        }

        public override IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IEnumerable<T>, IOrderedEnumerable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            return base.Get(filter, orderBy, includes).Where(t => t.Active);
        }

        public override IEnumerable<T> FilterWithPaging(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50)
        {
            return base.FilterWithPaging(filter, out total, index, size).Where(t => t.Active);
        }

        public override bool Contains(Expression<Func<T, bool>> predicate)
        {
            Func<T, bool> isActiveFunc = (t) => t.Active;
            Expression<Func<T, bool>> filterWithActive = Expression.Lambda<Func<T, bool>>(
                Expression.And(predicate.Body, Expression.Call(isActiveFunc.Method)));

            return base.Contains(filterWithActive);
        }

        public override void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                entity.Active = false;
                this.Update(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                LogEntityValidationException(dbEx);
            }
        }

        #endregion IRepository Methods
    }
}