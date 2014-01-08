using System;
using System.Linq;
using System.Linq.Expressions;

namespace Formall.Persistence
{
    public interface IRepositoryQuery<T> : IQueryable<Entity<T>>
        where T : class
    {
        IRepositoryQuery<T> Include(string path);
        IRepositoryQuery<T> Include<TProperty>(Expression<Func<T, TProperty>> path);
    }
}
