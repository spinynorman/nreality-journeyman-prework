using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDataStructures
{
    public class StringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return System.String.Compare(x, y, System.StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
