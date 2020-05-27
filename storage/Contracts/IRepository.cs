using System;
using System.Linq;
using System.Linq.Expressions;

namespace GameTrove.Storage.Contracts
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> query);
    }
}