using System;
using Microsoft.EntityFrameworkCore;

namespace handler.tests.Infrastructure
{
    public abstract class InMemoryContext<TContext>
        where TContext : DbContext
    {
        protected TContext Context;

        protected InMemoryContext()
        {
            var options = new DbContextOptionsBuilder<TContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            Context = (TContext)Activator.CreateInstance(typeof(TContext), options);
        }
    }
}