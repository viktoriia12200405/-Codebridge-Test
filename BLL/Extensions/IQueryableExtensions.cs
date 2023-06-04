using BLL.Models.Params;
using Common.Extensions;
using System.Linq.Expressions;

namespace BLL.Extensions;
public static class IQueryableExtensions
{
    public static IQueryable<T> GetPage<T>(this IQueryable<T> query, FilterParams @params)
    {
        if (@params is null)
            return query;

        return query.Skip((@params.PageNumber - 1) * @params.PageSize)
            .Take(@params.Limit);
    }

    public static IQueryable<T> OrderByAttribute<T>(this IQueryable<T> source, string attributeName, string order)
    {
        var entityType = typeof(T);
        var properties = entityType.GetProperties();

        var property = properties
            .FirstOrDefault(
                prop => prop.Name.Equals(attributeName,
                    StringComparison.OrdinalIgnoreCase));

        if (property == null)
            throw new ArgumentException($"PROPERTY_{attributeName}_NOT_FOUND_ON_TYPE_{entityType.Name}");

        var parameter = Expression.Parameter(entityType, "x");
        var propertyAccess = Expression.Property(parameter, property);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        order = order.IsNullOrEmpty() 
            ? "asc"
            : order;

        var orderByMethod = order == "asc" 
            ? "OrderBy" 
            : "OrderByDescending";

        MethodCallExpression orderByCall = Expression.Call(
            typeof(Queryable),
            orderByMethod,
            new[] { entityType, property.PropertyType },
            source.Expression,
            Expression.Quote(orderByExpression)
        );

        return source.Provider.CreateQuery<T>(orderByCall);
    }
}
