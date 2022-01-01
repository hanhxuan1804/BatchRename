using System;
using System.Collections.Generic;

namespace IRenameRules
{
    public interface IRenameRule
    {
        string Key { get; }
        string Description { get; set; }

        string Rename(string original);
        string Rename(string original, int index);

        public IRenameRule Clone(List<string> data);
        public string toString();
    }
}
