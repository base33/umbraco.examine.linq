using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Examine.Linq.Extensions
{
    public static class ClassTypes
    {
        public static bool IsWithinRange(this DateTime value, DateTime from, DateTime to)
        {
            return value > from && value < to;
        }
    }
}
