using System.Linq.Expressions;
using System.Reflection;

namespace App.Core.Helpers
{
    public static class OrderByHelper
    {
        // Erweiterungsmethode zum Sortieren einer IQueryable-Quelle nach einer Eigenschaft
        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(
            this IQueryable<TEntity> source,
            string orderByProperty,
            bool desc)
        {
            string command = desc ? "OrderBy" : "OrderByDescending";
            Type type = typeof(TEntity);
            PropertyInfo property = type.GetProperty(orderByProperty);

            // Erstellung der Parameter und Zugriff auf die Eigenschaft für den Ausdruck
            ParameterExpression parameter = Expression.Parameter(
                    type,
                    "p");
            MemberExpression propertyAccess = Expression.MakeMemberAccess(
                    parameter,
                    property);

            // Erstellung des Lambda-Ausdrucks für die Sortierung
            LambdaExpression orderByExpression = Expression.Lambda(
                    propertyAccess,
                    parameter);

            // Erstellung des Methodenaufrufs für die Sortierung
            MethodCallExpression resultExpression = Expression.Call(
                    typeof(Queryable),
                    command,
                    new Type[] { type, property.PropertyType },
                    source.Expression, Expression.Quote(orderByExpression));
            
            // Erzeugung der sortierten IQueryable-Quelle
            return (IOrderedQueryable<TEntity>)source.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}
