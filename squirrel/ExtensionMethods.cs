using System.Collections.Generic;
using System.Linq;

namespace squirrel
{
    public static class ExtensionMethods
    {
        public static T Head<T>(this List<T> list)
        {
            return list[0];
        }

        public static List<T> Tail<T>(this List<T> list)
        {
            return list.Skip(1).ToList();
        }

        public static string Capitalize(this string str)
        {
            return str.First().ToString().ToUpper() + str.Substring(1);
        }
    }
}
