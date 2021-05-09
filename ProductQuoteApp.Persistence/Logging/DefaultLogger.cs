using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class DefaultLogger : Logger
    {
        private readonly ILogRecordRepository logRepository;

        public DefaultLogger(ILogRecordRepository logRepo)
        {
            logRepository = logRepo;
        }

        public override void Error(string message)
        {
            base.Error(message);

            var log = new LogRecord
            {
                LogLevel = LogLevel.Error,
                ShortMessage = message,
                //CreatedOnUtc = DateTime.UtcNow
                CreatedOnUtc = DateTime.Now
            };
            logRepository.CreateAsync(log);
        }

        public override void Error(string fmt, params object[] vars)
        {
            base.Error(fmt, vars);

            var msg = String.Format(fmt, vars);
            var log = new LogRecord
            {
                LogLevel = LogLevel.Error,
                ShortMessage = msg,
                //CreatedOnUtc = DateTime.UtcNow
                CreatedOnUtc = DateTime.Now
            };
            logRepository.CreateAsync(log);
        }

        public override void Error(Exception exception, string fmt, params object[] vars)
        {
            base.Error(exception, fmt, vars);

            var msg = String.Format(fmt, vars);
            var fullMsg = "Exception Details=" + ExceptionUtils.FormatException(exception, includeContext: true);
            var log = new LogRecord
            {
                LogLevel = LogLevel.Error,
                ShortMessage = msg,
                FullMessage = fullMsg,
                //CreatedOnUtc = DateTime.UtcNow
                CreatedOnUtc = DateTime.Now
            };
            logRepository.CreateAsync(log);
        }

    }
}
