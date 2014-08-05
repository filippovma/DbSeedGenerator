using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using LeviySoft.Extensions;
using System.Collections.Generic;
using Patterns;

namespace DbSeedGenerator
{
    public static class DbSeedGenerator
    {
        public static Func<PropertyInfo, bool> IsNotCollection =
            info =>
                !info.PropertyType.ImplementsInterface(typeof(IList<>)) &&
                !info.PropertyType.ImplementsInterface(typeof(ICollection<>)) &&
                !info.PropertyType.ExtendsClass(typeof(List<>));

        public static Func<PropertyInfo, bool> ConsiderPrimitive =
            info =>
                info.PropertyType.IsPrimitive || info.PropertyType.IsValueType || info.PropertyType == typeof(string);

        public static Func<object, object> IdentityOf = o =>
        {
            if (o.GetType().IsPrimitive || o.GetType().IsValueType)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, @"Unknown primitive or value type ""{0}"".", o.GetType().Name));
            }
            var keyProperty = o.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(p => Attribute.IsDefined(p, typeof(KeyAttribute)));
            if (keyProperty == null)
            {
                throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, "Cannot find key for entity {0}", o.GetType().Name));
            }
            return keyProperty.GetValue(o);
        };

        public static Func<object, string> NonPrimitiveConvertor = o =>
        {
            var typeName = o.GetType().Name;
            var propertiesWithPublicSetters =
                o.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(
                        p =>
                            !Attribute.IsDefined(p, typeof(DiscardAttribute)) && !p.Name.EndsWith("Id") &&
                            IsNotCollection(p) && p.CanWrite && p.GetSetMethod(true).IsPublic).ToList();
            var primitiveProperties = propertiesWithPublicSetters.Where(ConsiderPrimitive);
            var notPrimitiveProperties = propertiesWithPublicSetters.Where(p => !ConsiderPrimitive(p));
            var primitivePropertiesInit = primitiveProperties.Select(p => Tuple.Create(p.Name, CodeProducer(p.GetValue(o))));
            var nonPrimitivePropertiesInit = notPrimitiveProperties.Where(p => p.GetValue(o) != null).Select(p => Tuple.Create(p.Name, string.Format(CultureInfo.InvariantCulture, "{0}_{1}", p.PropertyType.Name.ToLowerInvariant(), CodeProducer(IdentityOf(p.GetValue(o))))));
            var initializationStrings =
                primitivePropertiesInit.Concat(nonPrimitivePropertiesInit)
                    .Select(t => string.Format(CultureInfo.InvariantCulture, "{0} = {1}", t.Item1, t.Item2));
            return string.Format(CultureInfo.InvariantCulture, "var {0}_{1} = new {2} {{{3}}};",
                typeName.ToLowerInvariant(), CodeProducer(IdentityOf(o)), typeName, string.Join(", ", initializationStrings));
        };

        public static Func<object, string> CodeProducer = new Matcher<object, string>
        {
            {o => o is bool,        (object o) => ((bool)o).ToString(CultureInfo.InvariantCulture).ToLowerInvariant()},
            {o => o is int,         o => ((int)o).ToString(CultureInfo.InvariantCulture)},
            {o => o is Guid,        (object o) => string.Format(CultureInfo.InvariantCulture, @"new Guid(""{0}"")", o)},
            {o => o as string,      o => string.Format(CultureInfo.InvariantCulture, "@\"{0}\"", o.Replace(@"""", @""""""))},
            {o => o is DateTime,    o => string.Format(CultureInfo.InvariantCulture, "new DateTime({0})", ((DateTime) o).ToString("yyyy, M, d, H, m, s", CultureInfo.InvariantCulture))},
            {o => o is decimal,     o => string.Format(CultureInfo.InvariantCulture, "{0}m", ((decimal)o).ToString("f", CultureInfo.InvariantCulture))},
            {o => o is double,     o => ((double)o).ToString(CultureInfo.InvariantCulture)},
            {o => o is float,     o => string.Format(CultureInfo.InvariantCulture, "{0}f", ((float)o).ToString("f", CultureInfo.InvariantCulture))},
            {o => o == null,        (object o) => "null"},
            {_ => true,             (object o) => NonPrimitiveConvertor(o)}
        }.ToFunc();

        /// <summary>
        /// Use for generation code for single object.
        /// </summary>
        /// <param name="o">Object</param>
        /// <returns>Code lines</returns>
        public static List<string> GenCode(object o)
        {
            var tree = ObjectDecomposer.ObjectDecomposer.Decompose(o);
            var result = tree.ToStringList(CodeProducer);
            result.Reverse();
            return result;
        }

        /// <summary>
        /// Use for generation code for object list.
        /// </summary>
        /// <param name="objectList">List of objects.</param>
        /// <returns>Code lines</returns>
        public static List<string> GenCode(List<object> objectList)
        {
            var result = new List<string>();
            var variableList = new List<string>();

            foreach (var o in objectList)
            {
                foreach (var line in  GenCode(o))
                {
                    var varName = line.Split(new[] {'='}, 2)[0].Trim();
                    if (variableList.Contains(varName))
                    {
                        continue;
                    }
                    variableList.Add(varName);
                    result.Add(line);
                }
                
            }
            return result;
        } 
    }
}
