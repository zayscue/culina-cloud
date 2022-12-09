using System;
using System.Linq;
using System.Linq.Expressions;

public static class LINQExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Func<bool> condition, Expression<Func<T, bool>> predicate) where T : class
    {
        if (condition())
        {
            source = source.Where(predicate);
        }
        return source;
    }

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate) where T : class
    {
        if (condition)
        {
            source = source.Where(predicate);
        }
        return source;
    }
}