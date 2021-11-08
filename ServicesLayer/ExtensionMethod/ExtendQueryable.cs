using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ServicesLayer.ExtensionMethod;

namespace ServicesLayer.ExtensionMethod
{
    public static class ExtendQueryable
    {
        public static string CSVStringFormat<T>(this IQueryable<T> source)
        {
            var type = typeof(T);

            string myData = string.Empty;
            string myHeader = (string.Join(",", type.GetProperties().Select(pro => pro.Name).ToArray()));
            source.ToList().ForEach(cl =>
            {
                var lineArray = cl.GetType().GetProperties().Select(pro => pro.GetValue(cl)?.ToString()).ToArray();
                myData += string.Join(",", lineArray) + "\n";
            });
            return (myHeader + "\n" + myData);
        }
        public static IQueryable<T> FilterWithRange<T>(this IQueryable<T> source, int min, int max, params string[] propertiesName)
        {
            if (min == max && min == 0) return source;

            var parameter = Expression.Parameter(typeof(T), string.Empty);

            var minConstant = Expression.Constant(min, typeof(int));
            var maxConstant = Expression.Constant(max, typeof(int));

            var expressions = propertiesName.ToList().Select(property => new
            {
                GreaterThanEqual = Expression.GreaterThanOrEqual(Expression.Property(parameter, property), minConstant),
                LessThanOrEqual = Expression.LessThanOrEqual(Expression.Property(parameter, property), maxConstant)
            }).ToList();
            if (expressions != null)
            {
                var body = Expression.AndAlso(expressions[0].GreaterThanEqual, expressions[0].LessThanOrEqual);
                for (int index = 1; index < expressions.Count; index++)
                    body = Expression.AndAlso(expressions[index].GreaterThanEqual, expressions[index].LessThanOrEqual);
                var lambda = Expression.Lambda<Func<T, bool>>(body, new[] { parameter });
                return source.Where(lambda);
            }
            return null;
        }
        public static IQueryable<T> FilterByText<T>(this IQueryable<T> source, string text, params string[] propertiesName)
        {
            var type = typeof(T);
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
            var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
            ///--------------------------------------------------------------------/
            return source.Where(lambda);
        }
        public static IQueryable<T> OrderHelper<T>(IQueryable<T> source, string propertyName, bool orderAscendingDirection, bool subLevel)
        {
            if (string.IsNullOrEmpty(propertyName)) return source;

            var type = typeof(T);
            var property = type.GetProperty(propertyName);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            var resultExp = Expression.Call(typeof(Queryable), ((subLevel) ? "ThenBy" : "OrderBy") + (orderAscendingDirection ? string.Empty : "Descending"), new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName) => OrderHelper<T>(source, propertyName, true, false);

        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName) => OrderHelper<T>(source, propertyName, false, false);

        public static IQueryable<T> ThenBy<T>(this IQueryable<T> source, string propertyName) => OrderHelper<T>(source, propertyName, true, true);

        public static IQueryable<T> ThenByDescending<T>(this IQueryable<T> source, string propertyName) => OrderHelper<T>(source, propertyName, true, false);

    }
}
