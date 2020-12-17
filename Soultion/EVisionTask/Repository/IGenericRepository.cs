using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
namespace Repository
{
    public interface IGenericRepository<T>
        where T : class
    {
        T Add(T entity);
        void Delete(T entity);


        List<T> GetAll();
        IQueryable<T> GetAllQ();
        void Update(T entity);

        //public List<T> FindBy(Expression<Func<T, bool>> filter, Func<IQueryable, IOrderedQueryable>, params Expression<Func<T, object>>[] );
        //System.Linq.IQueryable<T> Include(System.Linq.Expressions.Expression<Func<T, object>> Includ );
        List<T> FindBy(Expression<Func<T, bool>> predicate);
        List<T> FindBy(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, Func<IQueryable<T>, IQueryable<T>> includes);
        IQueryable<T> FindByQ(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, Func<IQueryable<T>, IQueryable<T>> includes);

        IQueryable<T> Include(Expression<Func<T, object>> criteria);





    }
}