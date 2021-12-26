using IRenameRules;
using System;
using System.Collections.Generic;

namespace AddCounterRule
{
    public class CAddCounterRule : IRenameRule
    {
        int start;
        int step;
        public string Description { get; set; }

        public string Key => "addcounter";
        public CAddCounterRule()
        {
           
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

            string ex = result.Substring(id , result.Length - id);
            result = result.Substring(0, id);

            index = start + (index * step);
            result += index.ToString();//TODO: th�m pading 01,02,...
            result += ex;
            return result;
        }

        public IRenameRule Clone(List<string> data)
        {
            start = int.Parse(data[0]) ;
            step = int.Parse(data[1]);
            return new CAddCounterRule(start, step);
        }
    }
}
