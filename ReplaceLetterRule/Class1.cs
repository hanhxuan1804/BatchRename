using IRenameRules;
using System;
using System.Collections.Generic;

namespace ReplaceLetterRule
{
    public class CReplceLetterRule : IRenameRule
    {
        public string Replacer { get; set; }
        public string Description { get; set; }
        public List<string> Needles { get; set; }

        public CReplceLetterRule()
        {
            
        }
        public CReplceLetterRule(List<string> needles, string replacer)
        {
            Needles = needles;
            Replacer = replacer;
        }
        public string Key => "replaceletter";

        public string Rename(string original)
        {
            string result = original;

            foreach (var needle in Needles)
            {
                result = result.Replace(needle, Replacer);
            }

            return result;
        }

        public string Rename(string original, int index)
        {
            return original;
        }

        public IRenameRule Clone(List<string> data)
        {
            string replacer =data[data.Count - 1];
           data.RemoveAt(data.Count - 1);
           return new CReplceLetterRule(data, replacer);
        }
    }
}
