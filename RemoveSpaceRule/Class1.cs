using IRenameRules;
using System;
using System.Collections.Generic;

namespace RemoveSpaceRule
{
    public class CRemoveSpaceRule : IRenameRule
    {


        public string Description { get; set; }

        public CRemoveSpaceRule()
        {

        }
        public string Key => "removespace";


        public string Rename(string original)
        {
            string result = original;
            if (string.IsNullOrEmpty(result)) return original;
            result = result.Replace(" ", string.Empty);
            return result;
        }

        public string Rename(string original, int index)
        {
            return original;
        }

        public IRenameRule Clone(List<string> data)
        {
           return new CRemoveSpaceRule();
        }

        public string toString()
        {
            return $"{Key} decript: {Description}";
        }
    }
}
