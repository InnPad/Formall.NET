using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    public class RepositoryQuery<TResult> : IRepositoryQuery<TResult>
        where TResult : class
    {
        IQueryable<Entity<TResult>> _internal;

        public RepositoryQuery(IQueryable<Entity<TResult>> dbQuery)
        {
            _internal = dbQuery;
        }

        public IRepositoryQuery<TResult> Include(string path)
        {
            //_internal = System.Data.Entity.DbExtensions.Include<TResult>(_internal, path);

            return this;
        }

        public IRepositoryQuery<TResult> Include<TProperty>(Expression<Func<TResult, TProperty>> path)
        {
            //_internal = System.Data.Entity.DbExtensions.Include<TResult, TProperty>(_internal, path);

            return this;
        }

        public System.Collections.Generic.IEnumerator<Entity<TResult>> GetEnumerator()
        {
            return _internal.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _internal.GetEnumerator();
        }

        public Type ElementType
        {
            get { return _internal.ElementType; }
        }

        public Expression Expression
        {
            get { return _internal.Expression; }
        }

        public IQueryProvider Provider
        {
            get { return _internal.Provider; }
        }
    }
}
