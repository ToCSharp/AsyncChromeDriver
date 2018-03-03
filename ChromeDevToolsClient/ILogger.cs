using System;

namespace Zu.ChromeDevTools
{
    public class ILogger<T>
    {
        public virtual void LogTrace(string message, object[] args)
        {
            
        }

        public virtual void LogError(string message, object[] args)
        {

        }
    }
}