using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRestAPIBasic.Business.Models.Validations.Documents
{
    public class Utils
    {
        public static string OnlyNumber(string value)
        {
            var onlyNumber = "";
            foreach (var s in value)
            {
                if (char.IsDigit(s))
                {
                    onlyNumber += s;
                }
            }
            return onlyNumber.Trim();
        }
    }
}
