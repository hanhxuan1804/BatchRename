using IRenameRules;
using System;
using System.Collections.Generic;

namespace AddSuffixRule
{
    public class CAddSuffixRule : IRenameRule
    {
        public string Suffix { get; set; }
        public string Description { get; set; }
        public CAddSuffixRule()
        {
      
        }
        public CAddSuffixRule(string suffix)
        {
            Suffix = suffix;
        }

        public string Rename(string original)
        {
            string result = original;

            if (string.IsNullOrEmpty(result)) return original;
            int id = result.LastIndexOf('.');
            if (id == -1) return $"{result}{Suffix}";

            string ex = result.Substring(id, result.Length - id);
            result = result.Substring(0, id);

            result = $"{result}{Suffix}{ex}";

            return result;
        }
        public string Key => "addsuffix";


        public string Rename(string original, int index)
        {
            return original;
        }

        public IRenameRule Clone(List<string> data)
        {
            return new CAddSuffixRule(data[0]);
        }

        public string toString()
        {
            return $"{Key} {Suffix} decript: {Description}";
        }
    }
}
