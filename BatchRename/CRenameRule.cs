using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BatchRename
{

    public class CReplceExtensionRule : IRenameRule
    {
        public string Replacer { get; set; }
        public string Description { get ; set ; }

        public CReplceExtensionRule(string replacer)
        {
            Replacer = replacer;
        }
        public string key()
        {
            return "replaceExtension";
        }
       
        public string Rename(string original)
        {
            string result = original;
            if (string.IsNullOrEmpty(result)) return original;
            int index = result.LastIndexOf('.');
            if (index == -1) return original;
            result = result.Substring(0, index+1);
            result += Replacer;
            return result;
        }

        public string Rename(string original, int index)
        {
            return original;
        }
    }
    public class CAddCounterRule : IRenameRule
    {
        int start;
        int step;
        public string Description { get; set ; }

        public string key()
        {
           return "addcounterrule";
        }
        public CAddCounterRule(int cstart, int cstep)
        {
            start = cstart;
            step = cstep;
        }
        public string Rename(string original)
        {
            return original;
        }

        public string Rename(string original, int index)
        {
            string result = original;
            if (string.IsNullOrEmpty(result)) return original;
            int id = result.LastIndexOf('.');
            if (id == -1) return original;

            string ex=result.Substring(id+1, result.Length -1 - id);
            result = result.Substring(0, id);
            
            index = start + (index * step);
            result += index.ToString();//TODO: thêm pading 01,02,...
            result += ex;
            return result;
        }
       
    }
    public class CRemoveSpaceRule : IRenameRule
    {


        public string Description { get; set; }

        public CRemoveSpaceRule()
        {

        }
        public string key()
        {
            return "removespacerule";
        }

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
    }
    public class CReplceLetterRule : IRenameRule
    {
        public string Replacer { get; set; }
        public string Description { get; set; }
        public List<string> Needles { get; set; }

        public CReplceLetterRule(List<string> needles,string replacer)
        {
            Needles = needles;
            Replacer = replacer;
        }
        public string key()
        {
            return "replaceletter";
        }

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
    }
    public class CAddPrefixRule : IRenameRule
    {
        public string Prefix { get; set; }
        public string Description {get; set; }
        public CAddPrefixRule(string prefix)
        {
            Prefix = prefix;
        }

        public string Rename(string original)
        {
            string result = original;

            if (string.IsNullOrEmpty(result)) return original;
            int id = result.LastIndexOf('.');
            if (id == -1) return original;

            string ex = result.Substring(id + 1, result.Length - 1 - id);
            result = result.Substring(0, id);

            result = $"{Prefix}{result}{ex}";

            return result;
        }
        public string key()
        {
            return "addprefix";
        }

        public string Rename(string original, int index)
        {
            return original;
        }
    }
    public class CAddSuffixRule : IRenameRule
    {
        public string Suffix { get; set; }
        public string Description { get; set; }
        public CAddSuffixRule(string suffix)
        {
            Suffix = suffix;
        }

        public string Rename(string original)
        {
            string result = original;

            if (string.IsNullOrEmpty(result)) return original;
            int id = result.LastIndexOf('.');
            if (id == -1) return original;

            string ex = result.Substring(id + 1, result.Length - 1 - id);
            result = result.Substring(0, id);

            result = $"{result}{Suffix}{ex}";

            return result;
        }
        public string key()
        {
            return "addprefix";
        }

        public string Rename(string original, int index)
        {
            return original;
        }
    }
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
        public string key()
        {
            return "tolower";
        }

        public string Rename(string original, int index)
        {
            return original;
        }
    }
    public class CParcalCaseRule : IRenameRule
    {
        public string Description { get; set; }
        public CParcalCaseRule()
        {
        }

        public string Rename(string original)
        {
            return ToPascalCase(original);
        }
        public string key()
        {
            return "pascalcase";
        }

        public string Rename(string original, int index)
        {
            return original;
        }
        public string ToPascalCase(string original)
        {
            Regex invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
            Regex whiteSpace = new Regex(@"(?<=\s)");
            Regex startsWithLowerCaseChar = new Regex("^[a-z]");
            Regex firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
            Regex lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
            Regex upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

            // replace white spaces with undescore, then replace all invalid chars with empty string
            var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(original, "_"), string.Empty)
                // split by underscores
                .Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                // set first letter to uppercase
                .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
                // replace second and all following upper case letters to lower if there is no next lower (ABC -> Abc)
                .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
                // set upper case the first lower case following a number (Ab9cd -> Ab9Cd)
                .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))
                // lower second and next upper case letters except the last if it follows by any lower (ABcDEf -> AbcDef)
                .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));

            return string.Concat(pascalCase);
        }
    }
}

