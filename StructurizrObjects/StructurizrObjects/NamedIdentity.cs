using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace StructurizrObjects
{
    public static class NamedIdentity
    {
        private static Type[] ReferencedAssembliesTypesCache;

        public static string GetNameFromType(Type type)
        {
            var input = type.Name;
            return SpacesFromCamel(input);
        }

        public static string GetName<T>()
        {
            var input = typeof(T).Name;
            return SpacesFromCamel(input);
        }

        public static bool TryGetTypeFromExternalAssemblyByElementName(string name, out Type type)
        {
            if (ReferencedAssembliesTypesCache == null)
            {
                var path = AppContext.BaseDirectory;  // returns bin/debug path
                var directory = new DirectoryInfo(path);
                var dllFiles = directory.GetFiles("*.dll");
                ReferencedAssembliesTypesCache = dllFiles
                    .SelectMany(x => Assembly.LoadFile(x.FullName).GetTypes())
                    .ToArray();
            }

            var camelCase = CamelFromSpaces(name);
            type = ReferencedAssembliesTypesCache.FirstOrDefault(x => x.Name == camelCase);
            return type != null;
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

        public static string CamelFromSpaces(string value)
        {
            if (value.Length > 0)
            {
                bool makeUpper = true;
                var result = new List<char>();
                char[] array = value.ToCharArray();

                foreach (var item in array)
                {
                    if (makeUpper)
                    {
                        result.Add(char.ToUpper(item));
                        makeUpper = false;
                        continue;
                    }
                    if (item == ' ')
                    {
                        makeUpper = true;
                    }
                    else
                    {
                        result.Add(item);
                    }
                    
                }

                return new string(result.ToArray());
            }
            return value;
        }
    }
}
