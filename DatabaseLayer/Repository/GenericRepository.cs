using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer.Repository.Interface;
using DatabaseLayer.Context;
using System.Linq.Expressions;
using DatabaseLayer.ExceptionHandling;
using System.Reflection;

namespace DatabaseLayer.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        private readonly DatabaseContext _dbContext;
        public GenericRepository(DatabaseContext dbContext) => _dbContext = dbContext;
        public TEntity Get(int id) => _dbContext.Set<TEntity>().Find(id);
        public IQueryable<TEntity> GetAll() => _dbContext.Set<TEntity>();
        public int GetCounting() => _dbContext.Set<TEntity>().Count();
        public IQueryable<TEntity> GetPaging(int skip, int take) => _dbContext.Set<TEntity>().Skip(skip).Take(take);

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression) => _dbContext.Set<TEntity>().Where(expression);
        public TEntity Insert(TEntity entity, bool SaveChange)
        {
            _dbContext.Set<TEntity>().Add(entity);
            try
            {
                if (SaveChange) _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new CustomeException(e.Message);
            }
            return entity;
        }
        public void InsertRange(TEntity[] entities) => _dbContext.Set<TEntity>().AddRange(entities);
        public void Remove(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);
        public void RemoveRange(TEntity[] entities) => _dbContext.Set<TEntity>().RemoveRange(entities);
        public void Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);
        public void UpdateRange(TEntity[] entities) => _dbContext.Set<TEntity>().UpdateRange(entities);
        public PropertyInfo[] GetPropertiesInfor(TEntity entity) => typeof(TEntity).GetProperties();
        public PropertyInfo[] GetPropertiesInforByName(TEntity entity, params string[] name) => entity.GetType().GetProperties().Where(p => name.Contains(p.Name)).ToArray();
        public IQueryable<TEntity> FilterByText(IQueryable<TEntity> source, string text, params string[] propertiesName)
        {
            var type = typeof(TEntity);
            var list = type.GetProperties();
            var parameter = Expression.Parameter(type); // x
            var propertyList = type.GetProperties().Where(p => propertiesName.Contains(p.Name)); // x.[Property]
            var methodInfor = typeof(string).GetMethod("Contains", new Type[] { typeof(string), typeof(StringComparison) });
            var expressions = propertyList.ToList().Select(property => Expression.Call(
                Expression.Property(parameter, property),
                methodInfor,
                Expression.Constant(text, typeof(string)),
                Expression.Constant(StringComparison.InvariantCultureIgnoreCase, typeof(StringComparison))
            )).ToList();
            // wrap expression list into a body             
            var body = expressions[0];
            //for (int index = 1; index < expressions.Count; index++) body = Expression.Or(body, expressions[index]);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);
            ///--------------------------------------------------------------------/
            return source.ToList().AsQueryable().Where(lambda);
        }
    }
}
