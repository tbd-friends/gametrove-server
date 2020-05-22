using System;
using System.Linq;
using System.Linq.Expressions;
using GameTrove.Storage.Models;

namespace GameTrove.Storage.Contracts
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);

        IQueryable<TEntity> Query(Expression<Func<Title, bool>> expression);
    }
}