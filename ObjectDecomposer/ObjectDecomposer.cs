using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LeviySoft.Extensions;

namespace ObjectDecomposer
{
    public static class ObjectDecomposer
    {
        public static TreeNode Decompose(object o)
        {
            var properties = o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            properties = properties.Where(p => !(p.PropertyType.IsPrimitive || p.PropertyType.IsValueType || p.PropertyType == typeof(string) || p.PropertyType.ImplementsInterface(typeof(ICollection<>)) || p.PropertyType.ImplementsInterface(typeof(IList<>)) || p.PropertyType.ExtendsClass(typeof(List<>)))).ToArray();
            return new TreeNode(o, properties.Select(p => p.GetValue(o)).Where(v => v != null).Select(Decompose).ToList());
        }
    }
}
