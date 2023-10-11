using System.Linq.Expressions;

namespace VebtechTestTask.Application.Extensions;

public static class IQueryableExtensions
{
    public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string sortField, string sortOrder)
    {
        var param = Expression.Parameter(typeof(TEntity), "x");
        var property = Expression.Property(param, sortField);
        var lambda = Expression.Lambda<Func<TEntity, object>>(Expression.Convert(property, typeof(object)), param);

        return sortOrder == "asc" ? source.OrderBy(lambda) : source.OrderByDescending(lambda);
    }
}
