using System;
using System.Linq;
using System.Linq.Expressions;
using GameTrove.Storage.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GameTrove.Storage.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext Context { get; }

        protected Repository(DbContext context)
        {
            Context = context;
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {
            return Context.Set<TEntity>().Where(query);
        }
    }
}