using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StructurizrObjects
{
    public static class NamedIdentity
    {
        public static string GetNameFromType(Type type)
        {
            var input = type.Name;
            return SpacesFromCamel(input);
            //string[] words = Regex.Matches(input, "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)")
            //.OfType<Match>()
            //.Select(m => m.Value)
            //.ToArray();
            //return string.Join(" ", words);
        }

        public static string GetName<T>()
        {
            var input = typeof(T).Name;
            return SpacesFromCamel(input);
            //string[] words = Regex.Matches(input, "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)")
            //.OfType<Match>()
            //.Select(m => m.Value)
            //.ToArray();
            //return string.Join(" ", words);
        }

        public static string SpacesFromCamel(string value)
        {
            if (value.Length > 0)
            {
                var result = new List<char>();
                char[] array = value.ToCharArray();
                foreach (var item in array)
                {
                    if (char.IsUpper(item) && result.Count > 0)
                    {
                        result.Add(' ');
                    }
                    result.Add(item);
                }

                return new string(result.ToArray());
            }
            return value;
        }
    }
}
