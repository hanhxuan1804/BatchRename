using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    internal interface IRenameRule
    {
        string key();
        string Description { get; set; }

        string Rename(string original);
        string Rename(string original, int index);
    }
}
