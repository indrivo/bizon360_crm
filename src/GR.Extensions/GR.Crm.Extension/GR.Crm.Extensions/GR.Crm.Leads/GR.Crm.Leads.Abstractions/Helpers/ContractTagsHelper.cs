using System;
using System.Collections.Generic;
using GR.Crm.Leads.Abstractions.Models;

namespace GR.Crm.Leads.Abstractions.Helpers
{
    public static class ContractTagsHelper
    {
        private static Dictionary<string, Func<Agreement, Dictionary<string, object>, string>> CustomTags = new Dictionary<string, Func<Agreement,Dictionary<string,object>, string>>();


        public static Dictionary<string, string> GetInjectedStrings(Agreement agreement, Dictionary<string, object> additionalData = null)
        {
            var dict = new Dictionary<string, string>();
            foreach (var key in CustomTags)
            {
                var str = key.Value(agreement, additionalData);
                dict.Add(key.Key, str);
            }

            return dict;
        }

        public static void AddNewKey(string name, Func<Agreement, Dictionary<string, object>, string> handler)
        {
            if (CustomTags.ContainsKey(name)) return;
            CustomTags.Add(name, handler);
        }
    }
}
