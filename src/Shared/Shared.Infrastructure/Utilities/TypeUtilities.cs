using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shared.Infrastructure.Utilities
{
    public static class TypeUtilities
    {
        public static List<T?> GetAllPublicConstantValues<T>(this Type type)
        {
            return type
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T))
                .Select(x => (T?)x.GetRawConstantValue())
                .ToList();
        }

        public static List<string> GetNestedClassesStaticStringValues(this Type type)
        {
            var values = new List<string>();
            foreach (var prop in type.GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                object propertyValue = prop.GetValue(null) ?? throw new InvalidOperationException();
                values.Add(propertyValue.ToString() ?? string.Empty);
            }
        
            return values;
        }
        
        public static string GetGenericTypeName(this Type type)
        {
            if (type.IsGenericType)
            {
                string genericTypes = string.Join(",", type.GetGenericArguments().Select(GetGenericTypeName).ToArray());
                return $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }

            return type.Name;
        }
    }
}