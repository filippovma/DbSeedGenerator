using System.ComponentModel.DataAnnotations;

namespace DbSeedGenerator.Tests
{
    internal sealed class TopItem
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    internal sealed class SubItem1
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public TopItem TopItem { get; set; }
    }

    internal sealed class SubItem2
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public SubItem1 ParentSubItem { get; set; }
        public SubItem2Type Type { get; set; }

    }

    internal sealed class SubItem2Type
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    internal sealed class SubItem3
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public SubItem3Type Type { get; set; }
        public SubItem2 Parent { get; set; }
    }

    internal sealed class SubItem3Type
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    internal sealed class SubItem4
    {
        [Key]
        public int Id { get; set; }
        public SubItem4Type SubItem4Type { get; set; }
        public SubItem3 SubItem3 { get; set; }
        public string Name { get; set; }
    }
    
    internal sealed class SubItem4Type
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
