using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PandaMod.SpapiTools
{
    static class StaticToolClass
    {
        public static void Add<T>(ref T[] array, T toAdd)
        {
            List<T> list = array.ToList();
            list.Add(toAdd);
            array = list.ToArray<T>();
        }
        public static bool Contains<T>(this T[] array, T item)
        {
            return array.ToList().Contains(item);
        }
    }
}
