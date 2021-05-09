using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class ProductQuoteAppException : Exception
    {
        public string ResourceReferenceProperty { get; set; }

        public ProductQuoteAppException()
        {
        }

        public ProductQuoteAppException(string message)
        : base(message)
        {
        }

        public ProductQuoteAppException(string message, Exception inner)
        : base(message, inner)
        {
        }

        protected ProductQuoteAppException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ResourceReferenceProperty = info.GetString("ResourceReferenceProperty");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            info.AddValue("ResourceReferenceProperty", ResourceReferenceProperty);
            base.GetObjectData(info, context);
        }
    }
}
