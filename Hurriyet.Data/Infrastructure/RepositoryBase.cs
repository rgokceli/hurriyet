using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Hurriyet.Model;

namespace Hurriyet.Data.Infrastructure
{
    public abstract class RepositoryBase<T> where T : BaseModel
    {
        private HurriyetDataContext dataContext;
        private readonly IDbSet<T> dbset;
        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbset = DataContext.Set<T>();
        }


        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected HurriyetDataContext DataContext
        {
            get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        }
        public virtual void Add(T entity)
        {
            (entity as BaseModel).CreatedDate = DateTime.Now;
            (entity as BaseModel).UpdatedDate = DateTime.Now;
            dbset.Add(entity);
        }
        public virtual void Update(T entity)
        {
            (entity as BaseModel).UpdatedDate = DateTime.Now;
            dbset.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }


        public virtual void Delete(T entity)
        {
            entity.Deleted = true;
            dbset.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
            {
                obj.Deleted = true;
                dbset.Attach(obj);
                dataContext.Entry(obj).State = EntityState.Modified;
            }
        }
        public virtual T GetById(long id)
        {
            return dbset.Where(a => a.Id == id).Where(a => a.Deleted == false).SingleOrDefault();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbset.Where(a => a.Deleted == false);
        }
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).Where(a => a.Deleted == false);
        }
        public T Get(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).Where(a => a.Deleted == false).FirstOrDefault<T>();
        }


    }
}
