using IRenameRules;
using System;
using System.Collections.Generic;

namespace ReplaceExtensionRule
{

    public class CReplceExtensionRule : IRenameRule
    {
        public string Replacer { get; set; }
        public string Description { get; set; }

        public string Key => "replaceextension";

        public CReplceExtensionRule()
        {
        }
        public CReplceExtensionRule(string replacer)
        {
            Replacer = replacer;
        }


        public string Rename(string original)
        {
            string result = original;
            if (string.IsNullOrEmpty(result)) return original;
            int index = result.LastIndexOf('.');
            if (index == -1) return original;
            string ex = result.Substring(index, result.Length - index);
            if (ex == ".folder") return original;
            result = result.Substring(0, index + 1);
            result += Replacer;
            return result;
        }

        public string Rename(string original, int index)
        {
            return original;
        }

        public IRenameRule Clone(List<string> data)
        {
            var result = new CReplceExtensionRule(data[0]);

            return result;
        }

        public string toString()
        {
            return $"{Key} {Replacer} decript: {Description}";
        }
    }
}
