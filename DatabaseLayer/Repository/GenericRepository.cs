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
using System.ComponentModel.DataAnnotations;

namespace DatabaseLayer.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        private readonly DatabaseContext _dbContext;
        public GenericRepository(DatabaseContext dbContext) => _dbContext = dbContext;
        public TEntity Get(Guid id) => _dbContext.Set<TEntity>().Find(id);
        public IQueryable<TEntity> GetAll() => _dbContext.Set<TEntity>();
        public int GetCounting() => _dbContext.Set<TEntity>().Count();
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
        public IQueryable<TEntity> GetWithIDList(params Guid[] idValues)
        {
            var type = typeof(TEntity);
            var parameter = Expression.Parameter(type); // x
            // x.[Property] : get key (ID) of entity
            
            var proprertyInfor = type.GetProperties().Where(p => p.GetCustomAttribute<KeyAttribute>() != null).SingleOrDefault(); 
            var memberExpression = Expression.Property(parameter, proprertyInfor); // parameter.propertyInfor => x.ClassId...

            var expressions = idValues.ToList().Select(
                ID => Expression.Equal(memberExpression, Expression.Constant(ID, typeof(Guid)))
             );
            // wrap expression list into a body             
            // var body = expressions[0];
            // for (int index = 1; index < expressions.Count; index++) body = Expression.Or(body, expressions[index]);
            var body = expressions.Aggregate((pre, next) => Expression.Or(pre, next));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);
            ///--------------------------------------------------------------------/
            return _dbContext.Set<TEntity>().Where(lambda);
        }
    }
}
