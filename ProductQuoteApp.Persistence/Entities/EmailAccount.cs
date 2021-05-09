using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(EmailAccountMetaData))]
    public class EmailAccount
    {
        public int EmailAccountId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool IsDefault { get; set; }
        public string FriendlyName
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(this.DisplayName))
                    return this.Email + " (" + this.DisplayName + ")";
                return this.Email;
            }
        }
    }
}
