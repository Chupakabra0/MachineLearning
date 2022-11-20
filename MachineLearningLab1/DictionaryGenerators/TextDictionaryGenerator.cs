using MachineLearningLab1.TextLineGetter;
using MachineLearningLab1.Utils;

namespace MachineLearningLab1.DictionaryGenerators
{
    internal class TextDictionaryGenerator
    {
        public TextDictionaryGenerator(ITextLineGetter textLineGetter, IEnumerable<string>? exceptionStrings = null)
        {
            this.textLineGetter_   = textLineGetter;
            this.exceptionStrings_ = exceptionStrings
                                         ?.Select(s => s.GetRidOfRedundantSymbols()).OrderBy(s => s) ??
                                     null;
        }

        public Dictionary<string, int> GetFileDictionary()
        {
            var freq = new Dictionary<string, int>();

            while (true)
            {
                var line = this.textLineGetter_.GetTextLine();
                if (line is "")
                {
                    continue;
                }
                if (line is null)
                {
                    break;
                }
                line = line.GetRidOfRedundantSymbols();

                foreach (var word in line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!exceptionStrings_?.Contains(word) ?? true)
                    {
                        if (!freq.TryAdd(word, 1))
                        {
                            ++freq[word];
                        }
                    }
                }
            }

            return freq;
        }

        private ITextLineGetter      textLineGetter_;
        private IEnumerable<string>? exceptionStrings_;
    }
}
