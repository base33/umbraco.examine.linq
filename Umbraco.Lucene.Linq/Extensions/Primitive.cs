using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Lucene.Linq.Extensions
{
    public static class Primitive
    {
        //public static int Boost(this int value, int boost)
        //{
        //    return value;
        //}

        //public static string Boost(this string value, int boost)
        //{
        //    return value;
        //}

        public static bool Boost(this bool value, int boost)
        {
            return value;
        }

        public static bool Fuzzy(this bool value, double fuzzy)
        {
            return value;
        }

        public static bool ContainsAll(this string value, params string[] values)
        {
            string valueLower = value.ToLower();
            foreach(string item in values)
            {
                if(valueLower.IndexOf(item) < 0)
                    return false;
            }
            return true;
        }

        public static bool ContainsAny(this string value, params string[] values)
        {
            string valueLower = value.ToLower();
            foreach(string item in values)
            {
                if (valueLower.IndexOf(item) >= 0)
                    return true;
            }
            return false;
        }
    }
}
