using IRenameRules;
using System;
using System.Collections.Generic;

namespace ToLowerRule
{
    public class CToLowerRule : IRenameRule
    {
        public string Description { get; set; }
        public CToLowerRule()
        {
        }

        public string Rename(string original)
        {
            string result = original.ToLower();
            return result;
        }
        public string Key=>"tolower";
        

        public string Rename(string original, int index)
        {
            return original;
        }

        public IRenameRule Clone(List<string> data)
        {
            return new CToLowerRule();
        }
    }
}
