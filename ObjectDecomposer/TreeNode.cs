using System;
using System.Collections.Generic;

namespace ObjectDecomposer
{
    public sealed class TreeNode
    {
        public TreeNode(object value, IList<TreeNode> subNodes)
        {
            Value = value;
            Nodes = subNodes;
        }
        public object Value { get; private set; }
        public IList<TreeNode> Nodes { get; private set; }
        public List<string> ToStringList(Func<object, string> convertor)
        {
            var result = new List<string>
            {
                convertor(Value)
            };
            if (Nodes == null)
            {
                return result;
            }
            foreach (var node in Nodes)
            {
                result.AddRange(node.ToStringList(convertor));
            }
            return result;
        }

    }
}
