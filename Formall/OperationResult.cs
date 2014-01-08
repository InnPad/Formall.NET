using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Formall
{
    public abstract class OperationResult : IResult, IAsyncResult
    {
        private bool _asynchronous;

        protected OperationResult(bool async)
        {
            _asynchronous = async;
        }

        public object AsyncState
        {
            get { throw new NotImplementedException(); }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { throw new NotImplementedException(); }
        }

        public bool CompletedSynchronously
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAsynchronous
        {
            get { return _asynchronous; }
        }

        public bool IsCompleted
        {
            get { throw new NotImplementedException(); }
        }

        public virtual bool IsSuccess
        {
            get { return true; }
        }

        public abstract void Write(Stream stream);

        public abstract void Write(TextWriter writer);
    }
}
