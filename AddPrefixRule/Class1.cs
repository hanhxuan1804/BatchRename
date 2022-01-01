using IRenameRules;
using System;
using System.Collections.Generic;

namespace AddPrefixRule
{
    public class CAddPrefixRule : IRenameRule
    {
        public string Prefix { get; set; }
        public string Description { get; set; }
        public CAddPrefixRule()
        {
        }
        public CAddPrefixRule(string prefix)
        {
            Prefix = prefix;
        }

        public string Rename(string original)
        {
            string result = original;

            if (string.IsNullOrEmpty(result)) return original;
            int id = result.LastIndexOf('.');
            if (id == -1) return $"{Prefix}{result}";

            string ex = result.Substring(id, result.Length - id);
            result = result.Substring(0, id);

            result = $"{Prefix}{result}{ex}";

            return result;
        }
        public string Key => "addprefix";


        public string Rename(string original, int index)
        {
            return original;
        }

        public IRenameRule Clone(List<string> data)
        {
            return new CAddPrefixRule(data[0]);
        }

        public string toString()
        {
            return $"{Key} {Prefix} decript: {Description}";
        }
    }
}
