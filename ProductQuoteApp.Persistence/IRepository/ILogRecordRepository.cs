using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface ILogRecordRepository : IDisposable
    {
        void Create(LogRecord logRecordToAdd);
        Task CreateAsync(LogRecord logRecordToAdd);
        Task DeleteAsync(int logRecordID);
        Task<List<LogRecord>> FindLogRecordsAsync();
        Task<LogRecord> FindLogRecordByIDAsync(int logRecordID);
    }
}
