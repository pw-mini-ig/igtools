using System;
using System.Collections.Generic;
using System.Text;

namespace IgTool
{
    static class Extensions
    {
        public static IEnumerable<int> AllIndexesOf(this string haystack, string needle)
        {
            if (haystack == null) throw new ArgumentNullException(nameof(haystack));
            if (needle == null) throw new ArgumentNullException(nameof(needle));

            for (int index = 0; ; index++)
            {
                index = haystack.IndexOf(needle, index);
                if (index == -1) yield break;
                yield return index;
            }
        }

        public static StringBuilder Indent(this StringBuilder sb, int amount = 1)
        {
            for (int i = 0; i < amount; i++)
                sb.Append("  ");
            return sb;
        }
    }
}
