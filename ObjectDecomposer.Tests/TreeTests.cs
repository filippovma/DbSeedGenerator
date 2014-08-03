using System.Collections.Generic;
using Xunit;

namespace ObjectDecomposer.Tests
{
    public class TreeTests
    {
        

        [Fact]
        public void ShouldCorrentFlatTwoChainObjects()
        {
            var topItem = new TopItem();
            var subItem21 = new SubItem21();
            var subItem1 = new SubItem1 { Parent = topItem, SubItem21 = subItem21 };
            var subItem22 = new SubItem22 { Parent = subItem1 };
            var subItem31 = new SubItem31 { Parent = subItem22 };
            

            var tree = new Tree(new TreeNode(topItem, new List<TreeNode>
            {
                new TreeNode(subItem1, new List<TreeNode>
                {
                    new TreeNode(subItem21, null),
                    new TreeNode(subItem22, new List<TreeNode>
                    {
                        new TreeNode(subItem31, null)
                    })
                })
            }));
            var list = tree.ToStringList(o => o.GetType().Name);
            
            Assert.Equal("TopItem", list[0]);
            Assert.Equal("SubItem1", list[1]);
            Assert.Equal("SubItem21", list[2]);
            Assert.Equal("SubItem22", list[3]);
            Assert.Equal("SubItem31", list[4]);
            

        }
    }
}
