using Xunit;

namespace ObjectDecomposer.Tests
{
    public sealed class ObjectDecomposerTests
    {
        [Fact]
        public void ShouldCorrectDecomposeObject()
        {
            var topItem = new TopItem {Id = 1};
            var subItem21 = new SubItem21 { Id = 2};
            var subItem1 = new SubItem1 { Id = 3, Parent = topItem, SubItem21 = subItem21 };
            var subItem22 = new SubItem22 { Id = 4, Parent = subItem1 };
            var subItem31 = new SubItem31 { Id = 5, Parent = subItem22 };

            var result = ObjectDecomposer.Decompose(subItem31);

            Assert.Equal(5, ((SubItem31)result.Value).Id);
            Assert.Equal(4, ((SubItem22)result.Nodes[0].Value).Id);
            Assert.Equal(3, ((SubItem1)result.Nodes[0].Nodes[0].Value).Id);
            Assert.Equal(2, ((SubItem21)result.Nodes[0].Nodes[0].Nodes[1].Value).Id);
            Assert.Equal(1, ((TopItem)result.Nodes[0].Nodes[0].Nodes[0].Value).Id);
            
        }
    }
}
