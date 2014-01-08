using System;
using System.IO;

namespace Formall.Persistence
{
    using Formall.Reflection;
    
    public interface IEntity : IDocument
    {
        dynamic Data { get; }

        Guid Id { get; }

        Model Model { get; }

        IRepository Repository { get; }

        IResult Delete();

        T Get<T>() where T : class;

        IResult Refresh();

        IResult Set<T>(T value) where T : class;

        IResult Patch(object data);

        IResult Update(object data);

        void WriteJson(Stream stream);

        void WriteJson(TextWriter writer);
    }
}
