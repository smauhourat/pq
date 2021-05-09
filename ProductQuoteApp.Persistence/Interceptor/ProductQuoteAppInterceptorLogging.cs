using ProductQuoteApp.Logging;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;

namespace ProductQuoteApp.Persistence
{
    public class ProductQuoteAppInterceptorLogging: DbCommandInterceptor
    {
        private readonly ILogger _logger = new DefaultLogger(new LogRecordRepository());

        private readonly Stopwatch _stopwatch = new Stopwatch();

        public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            base.ScalarExecuting(command, interceptionContext);
            _stopwatch.Restart();
        }

        public override void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            
            _stopwatch.Stop();
            if (interceptionContext.Exception != null)
            {
                _logger.Error(interceptionContext.Exception, "Error executing command: {0} :: Parameters: {1} :: Message: {2}", command.CommandText, CommandParamToString(command.Parameters), interceptionContext.Exception.Message);
            }
            else
            {
                _logger.TraceApi("SQL Database", "ProductQuoteAppInterceptor.ScalarExecuted", _stopwatch.Elapsed, "Command: {0}: Parameters: {1}", command.CommandText, CommandParamToString(command.Parameters));
            }
            base.ScalarExecuted(command, interceptionContext);
        }

        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            base.NonQueryExecuting(command, interceptionContext);
            _stopwatch.Restart();
        }

        public override void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            _stopwatch.Stop();
            if (interceptionContext.Exception != null)
            {
                _logger.Error(interceptionContext.Exception, "Error executing command: {0}: Parameters: {1}", command.CommandText, CommandParamToString(command.Parameters));
            }
            else
            {
                _logger.TraceApi("SQL Database", "ProductQuoteAppInterceptor.NonQueryExecuted", _stopwatch.Elapsed, "Command: {0}: Parameters: {1}", command.CommandText, CommandParamToString(command.Parameters));
            }
            base.NonQueryExecuted(command, interceptionContext);
        }

        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            base.ReaderExecuting(command, interceptionContext);
            _stopwatch.Restart();
        }

        public override void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            _stopwatch.Stop();
            if (interceptionContext.Exception != null)
            {
                _logger.Error(interceptionContext.Exception, "Error executing command: {0}: Parameters: {1}", command.CommandText, CommandParamToString(command.Parameters));
            }
            else
            {
                _logger.TraceApi("SQL Database", "ProductQuoteAppInterceptor.ReaderExecuted", _stopwatch.Elapsed, "Command: {0}: Parameters: {1}", command.CommandText, CommandParamToString(command.Parameters));
            }
            base.ReaderExecuted(command, interceptionContext);
        }

        private string CommandParamToString(DbParameterCollection paramCol)
        {
            string ret = "";

            if (paramCol != null)
            {
                foreach (DbParameter item in paramCol)
                {
                    ret += item.ParameterName + "=" + item.Value.ToString() + "; ";
                }
            }

            return ret;
        }
            
    }
}
