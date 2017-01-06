using System.Collections.Generic;
using System.Linq;

namespace Squirrel
{
    public static class ExtensionMethods
    {
        public static T Head<T>(this List<T> list) => list[0];

        public static List<T> Tail<T>(this List<T> list) => list.Skip(1).ToList();
    }
}
