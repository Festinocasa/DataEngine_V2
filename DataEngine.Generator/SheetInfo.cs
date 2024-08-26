namespace DataEngine.Generator
{
    internal class SheetInfo
    {
        public string SheetModelName { get; set; }
        public int ElemCount { get; set; }
        public int ElemByteLen { get; set; }
        public List<string> FieldStrings { get; set; } = new List<string>();
    }
}