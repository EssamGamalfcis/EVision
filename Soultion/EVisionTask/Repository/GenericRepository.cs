using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;

using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Data.Common;
using Domain;

using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly CatalogContext context;
        private DbSet<T> db;
        public GenericRepository(CatalogContext context)
        {
            this.context = context;
            db = context.Set<T>();
        }
        public List<T> GetAll()
        {
            return db.ToList();
        }
        public IQueryable<T> GetAllQ()
        {
            return db;
        }
        public List<T> FindBy(Expression<Func<T, bool>> predicate)
        {

            return db.Where(predicate).ToList();
        }

        public List<T> FindBy(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            return QueryDb(filter, orderBy, includes).ToList();


        }
        public IQueryable<T> FindByQ(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            return QueryDb(filter, orderBy, includes);


        }

        public T Add(T entity)
        {

            db.Add(entity);
            context.SaveChanges();
            return entity;
        }
        public void Update(T entity)
        {
            // db.Attach(entity);

            db.Update(entity);
            context.SaveChanges();
        }

        public void Delete(T entity)
        {

            db.Remove(entity);
            context.SaveChanges();
        }
        public IQueryable<T> Include(Expression<Func<T, object>> criteria)
        {
            return db.Include(criteria);
        }


        protected IQueryable<T> QueryDb(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, Func<IQueryable<T>, IQueryable<T>> includes)
        {
            IQueryable<T> query = db;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }


    }
}
