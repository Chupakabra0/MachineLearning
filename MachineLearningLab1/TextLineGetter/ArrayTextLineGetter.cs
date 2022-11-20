namespace MachineLearningLab1.TextLineGetter
{
    internal class ArrayTextLineGetter : ITextLineGetter
    {
        public ArrayTextLineGetter(IEnumerable<string> strings)
        {
            this.stringArray_ = strings.ToArray();
            this.index_ = 0;
        }
        public string? GetTextLine()
        {
            return this.index_ < this.stringArray_.Length ?
                this.stringArray_[this.index_++] :
                null;
        }

        public void Reset()
        {
            this.index_ = 0;
        }

        private string[] stringArray_;
        private int      index_;
    }
}
