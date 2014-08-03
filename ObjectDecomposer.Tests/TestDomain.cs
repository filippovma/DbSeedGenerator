namespace ObjectDecomposer.Tests
{
    internal sealed class TopItem
    {
        public int Id { get; set; }
    }

    internal sealed class SubItem1
    {
        public int Id { get; set; }
        public TopItem Parent { get; set; }
        public SubItem21 SubItem21 {  get; set; }
    }

    internal sealed class SubItem21
    {
        public int Id { get; set; }
    }

    internal sealed  class SubItem22
    {
        public int Id { get; set; }
        public SubItem1 Parent { get; set; }
    }

    internal sealed  class SubItem31
    {
        public int Id { get; set; }
        public SubItem22 Parent { get; set; }

    }
}
