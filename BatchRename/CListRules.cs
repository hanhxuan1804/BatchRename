using IRenameRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class CListRules
    {
        public string Name { get; set; }

        public List<IRenameRule> Rules { get; set; }
    }
}
