using System;
using System.Collections.Generic;

namespace ObjectDecomposer
{
    public sealed class Tree
    {
        public Tree(TreeNode root)
        {
            Root = root;
        }
        public TreeNode Root { get; private set; }
        public List<string> ToStringList(Func<object, string> convertor)
        {
            return Root.ToStringList(convertor);
        }
    }
}
