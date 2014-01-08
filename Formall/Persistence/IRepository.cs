using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    using Formall.Reflection;

    public interface IRepository
    {
        IDataContext Context { get; }

        Model Model { get; }

        IEntity Create(object data);

        IResult Delete(Guid id);

        IEntity Read(Guid id);

        IEntity[] Read(int skip, int take);

        /// <summary>
        /// Remove from a list
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IResult Remove(Guid id, string field, string value);

        IResult Patch(Guid id, object data);

        IResult Update(Guid id, object data);
    }

    public interface IRepository<T> : IDisposable
        where T : class
    {
        bool AutoSave { get; set; }

        IRepositoryQuery<T> Select { get; }

        void Add(Guid id, T item);

        void AddOrUpdate(Guid id, T item);

        void Delete(Guid id, T item);

        void Save();
    }
}
