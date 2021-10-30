using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Repository.Interface
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> GetAll();        
        void Update(TEntity entity);
        void UpdateRange(TEntity[] entities);
        void Remove(TEntity entity);
        void RemoveRange(TEntity[] entities);
        public TEntity Insert(TEntity entity, bool SaveChange);
        void InsertRange(TEntity[] entities);
        IQueryable<TEntity> GetPaging(int skip, int take);
        PropertyInfo[] GetPropertiesInfor(TEntity entity);
        PropertyInfo[] GetPropertiesInforByName(TEntity entity, params string[] name);
        IQueryable<TEntity> FilterByText(IQueryable<TEntity> source, string text, params string[] propertiesName);        
    }
}
