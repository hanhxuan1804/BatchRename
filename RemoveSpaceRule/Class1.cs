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
            int id = result.LastIndexOf('.');
            if (id != -1) return original.Trim();
            string ex= result.Substring(id + 1);
            result = result.Substring(0, id).Trim();
            
            return $"{result}{ex}";
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
