using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rowa.Api.Interfaces;

namespace Rowa.Api.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected DatabaseContext DatabaseContext { get; set; }

        public BaseRepository(DatabaseContext dbContext)
        {
            DatabaseContext = dbContext == null || dbContext.Database.Connection.State != System.Data.ConnectionState.Open
                ? new DatabaseContext()
                : dbContext;
        }

        public BaseRepository() : this(null)
        {
        }

        public virtual T Add(T newItem)
        {
            var addedItem = DatabaseContext.Set<T>().Add(newItem);
            DatabaseContext.SaveChanges();

            return addedItem;
        }

        public virtual void Update(T updatedItem)
        {
            DatabaseContext.SaveChanges();
        }
    }
}