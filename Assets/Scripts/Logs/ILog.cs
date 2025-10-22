using System;
using Infrastructure;

namespace Logs
{
    public interface ILog : ISystem
    {
        void Debug(Func<string> callback, object sender = null);

        void Error(Func<string> callback);
        void Warning(Func<string> callback, object sender = null);
    }
}