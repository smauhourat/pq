using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    /// <summary>
    /// Represents a log record
    /// </summary>
    public class LogRecord
    {
        public int LogRecordID { get; set; }
        /// <summary>
        /// Gets or sets the log level identifier
        /// </summary>
        public int LogLevelId { get; set; }

        /// <summary>
        /// Gets or sets the short message
        /// </summary>
        public string ShortMessage { get; set; }

        /// <summary>
        /// Gets or sets the full exception
        /// </summary>
        public string FullMessage { get; set; }

        /// <summary>
        /// Gets or sets the IP address
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        //public int? CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the page URL
        /// </summary>
        public string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the referrer URL
        /// </summary>
        public string ReferrerUrl { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the log level
        /// </summary>
        [NotMapped]
        public LogLevel LogLevel
        {
            get
            {
                return (LogLevel)this.LogLevelId;
            }
            set
            {
                this.LogLevelId = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the customer
        /// </summary>
        //public virtual Customer Customer { get; set; }
    }

    /// <summary>
    /// Represents a log level
    /// </summary>
    public enum LogLevel
    {
        Debug = 10,
        Information = 20,
        Warning = 30,
        Error = 40,
        Fatal = 50
    }
}
