using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Examine.Linq.Extensions
{
    public static class Primitive
    {
        /// <summary>
        /// Boost results that meet this condition
        /// </summary>
        /// <param name="value"></param>
        /// <param name="boost"></param>
        /// <returns></returns>
        public static bool Boost(this bool value, int boost)
        {
            return value;
        }

        /// <summary>
        /// Fuzzy searches based on the Levenshtein Distance.  So - similarly spelt words.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fuzzy">0 - 1 (e.g. 0.8)</param>
        /// <returns></returns>
        public static bool Fuzzy(this bool value, double fuzzy)
        {
            return value;
        }

        /// <summary>
        /// Whether the field contains the text
        /// </summary>
        /// <param name="value"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool Contains(this string value, string test)
        {
            return value.Contains(value);
        }

        /// <summary>
        /// Whether the field contains all the values
        /// </summary>
        /// <param name="value"></param>
        /// <param name="values"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Whether the field contains at least one or more
        /// </summary>
        /// <param name="value"></param>
        /// <param name="values"></param>
        /// <returns></returns>
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

        public static bool IsAny(this int value, params int[] values)
        {
            foreach(int val in values)
            {
                if (value == val)
                    return true;
            }
            return false;
        }
    }
}
