using System;
using System.Diagnostics;

namespace ProductQuoteApp.Logging
{
    public class Logger : ILogger
    {
        //
        // Warning - trace information within the application 

        public void Information(string message)
        {
            Trace.TraceInformation(message);
        }

        public void Information(string fmt, params object[] vars)
        {
            Trace.TraceInformation(fmt, vars);
        }

        public void Information(Exception exception, string fmt, params object[] vars)
        {
            Trace.TraceInformation(string.Format(fmt, vars) + ";Exception Details={0}", ExceptionUtils.FormatException(exception, includeContext: true));
        }

        //
        // Warning - trace warnings within the application 

        public void Warning(string message)
        {
            Trace.TraceWarning(message);
        }

        public void Warning(string fmt, params object[] vars)
        {
            Trace.TraceWarning(fmt, vars);
        }

        public void Warning(Exception exception, string fmt, params object[] vars)
        {
            Trace.TraceWarning(string.Format(fmt, vars) + ";Exception Details={0}", ExceptionUtils.FormatException(exception, includeContext: true));
        }

        //
        // Error - trace fatal errors within the application 

        public virtual void Error(string message)
        {
            Trace.TraceError(message);
        }

        public virtual void Error(string fmt, params object[] vars)
        {
            Trace.TraceError(fmt, vars);
        }

        public virtual void Error(Exception exception, string fmt, params object[] vars)
        {
            Trace.TraceError(string.Format(fmt, vars) + ";Exception Details={0}", ExceptionUtils.FormatException(exception, includeContext: true));
        }

        //
        // TraceAPI - trace inter-service calls (including latency)

        public virtual void TraceApi(string componentName, string method, TimeSpan timespan)
        {
            TraceApi(componentName, method, timespan, "");
        }

        public virtual void TraceApi(string componentName, string method, TimeSpan timespan, string fmt, params object[] vars)
        {
            TraceApi(componentName, method, timespan, string.Format(fmt, vars));
        }

        public virtual void TraceApi(string componentName, string method, TimeSpan timespan, string properties)
        {
            string message = String.Concat("component:", componentName, ";method:", method, ";timespan:", timespan.ToString(), ";properties:", properties);
            Trace.TraceInformation(message);
        }
    }

}
