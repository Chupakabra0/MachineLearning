namespace MachineLearningLab1.TextLineGetter
{
    internal class FileTextLineGetter : ITextLineGetter
    {
        public FileTextLineGetter(FileInfo fileInfo)
        {
            this.Init(fileInfo.OpenRead());
        }

        public FileTextLineGetter(FileStream fileReadStream)
        {
            this.Init(fileReadStream);
        }

        ~FileTextLineGetter()
        {
            this.streamReader_.Close();
        }

        public string? GetTextLine()
        {
            return streamReader_.ReadLine();
        }

        public void Reset()
        {
            streamReader_.DiscardBufferedData();
            streamReader_.BaseStream.Seek(0, SeekOrigin.Begin);
        }

        private StreamReader streamReader_;

        private void Init(FileStream fileReadStream)
        {
            if (fileReadStream.CanRead)
            {
                this.streamReader_ = new StreamReader(fileReadStream);
            }
            else
            {
                throw new Exception("FileStream opened at not read");
            }
        }
    }
}
