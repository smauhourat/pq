using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Helpers
{
    public static class EpPlusExtension
    {
        public static int ColumnLetterToColumnIndex(string columnLetter)
        {
            columnLetter = columnLetter.ToUpper();
            int sum = 0;

            for (int i = 0; i < columnLetter.Length; i++)
            {
                sum *= 26;
                sum += (columnLetter[i] - 'A' + 1);
            }
            return sum;
        }
    }
}