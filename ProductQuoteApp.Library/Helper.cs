using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Library
{
    public static class Helper
    {
        public static decimal RoundDecimal(decimal value, int decimals)
        {
            return Math.Round(value, decimals);
        }

        /// <summary>
        /// La cantidad por defecto de decimales es 4 (cuatro)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal RoundDecimal(decimal value)
        {
            return RoundDecimal(value, 4);
        }


    }
}
