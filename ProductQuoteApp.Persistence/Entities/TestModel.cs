using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class TestModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        //[Range(typeof(DateTime), "2014-12-20", "2014-12-21")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        //[Range(typeof(decimal), "20", "25.99")]
        public Decimal Amount { get; set; }
    }
}
