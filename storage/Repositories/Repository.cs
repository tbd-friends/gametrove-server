using System;
using System.Linq;
using System.Linq.Expressions;
using GameTrove.Storage.Contracts;
using GameTrove.Storage.Models;

namespace GameTrove.Storage.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected Repository() // Context comes in here
        {
            
        }

        public void Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Query(Expression<Func<Title, bool>> expression)
        {
            throw new NotImplementedException(); 
        }
    }
}