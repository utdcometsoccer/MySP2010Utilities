using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections;

namespace MySP2010Utilities
{
    public static class ValidationHelper
    {
        public static void RequireNotNullOrEmpty(this string value, string argumentName)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("The value can't be null or empty", argumentName);
        }

        public static void RequireNotNull(this object value, string argumentName)
        {
            if (null == value)
                throw new ArgumentNullException(argumentName, "The value can't be null");
        }

        public static void RequireNotEmpty<T>(this IEnumerable<T> value, string argumentName)
        {
            RequireNotNull(value, argumentName);
            if (value.Count() <= 0)
            {
                throw new ArgumentException(string.Format("{0} cannot be empty!", argumentName),argumentName);
            }
        }

        public static void Require<T, E>(this T value, bool condition) where E : Exception, new()
        {
            if (!condition)
            {
                throw new E();
            }
        }

        public static void Require<T>(this T value, bool condtion, string argumentName)
        {
            if (!condtion)
            {
             throw new Exception(string.Format("validation failed for {0}",argumentName));   
            }
        }
    }
}
