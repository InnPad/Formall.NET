using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall
{
    public interface IResult
    {
        /// <summary>
        /// Indicate that it is an asynchronous operation and that you should 
        /// typecast to System.IAsyncResult in order to query the status.
        /// </summary>
        bool IsAsynchronous { get; }

        bool IsSuccess { get; }

        void Write(Stream stream);

        void Write(TextWriter writer);
    }
}