using System;
using Infrastructure;

namespace Logs
{
    public interface ILog
    {
        void Debug(Func<string> callback, object sender = null);

        void Error(Func<string> callback);
        void Warning(Func<string> callback, object sender = null);
    }
}