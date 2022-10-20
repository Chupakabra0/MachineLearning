using CommandLine;
using KnowledgePicker.WordCloud;
using KnowledgePicker.WordCloud.Coloring;
using KnowledgePicker.WordCloud.Drawing;
using KnowledgePicker.WordCloud.Layouts;
using KnowledgePicker.WordCloud.Primitives;
using KnowledgePicker.WordCloud.Sizers;
using MachineLearningLab1.DictionaryGenerators;
using MachineLearningLab1.TextLineGetter;
using SkiaSharp;

namespace MachineLearningLab1
{
    internal class Program
    {
        internal class Options
        {
            [Option('i', "input", Required = true, HelpText = "Set path to input text-file, that will be analyzed")]
            public string InputPathStr { get; set; }

            [Option('o', "output", Required = true, HelpText = "Set path to output png-file, where the word cloud will be saved")]
            public string OutputPathStr { get; set; }
        }

        private static void ParseOptionsAndRun(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Run)
                .WithNotParsed(HandleParseError);
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            throw new Exception("FUCK!");
        }

        private static void Run(Options options)
        {
            var exceptions = new[]
            {
                "the", "a", "to", "if", "is", "it", "of", "and", "or", "an", "as", "i", "me", "my",
                "we", "our", "ours", "you", "your", "yours", "he", "she", "him", "his", "her", "hers",
                "its", "they", "them", "their", "what", "which", "who", "whom", "this", "that", "am",
                "are", "was", "were", "be", "been", "being", "have", "has", "had", "do", "does", "did",
                "but", "at", "by", "with", "from", "here", "when", "where", "how", "all", "any", "both",
                "each", "few", "more", "some", "such", "no", "nor", "too", "very", "can", "will", "just",
                "on", "in"
            };

            var fileDic = new TextDictionaryGenerator(new FileTextLineGetter(new FileInfo(options.InputPathStr)), exceptions);
            // TODO: modernize sorting comparator
            var freq = fileDic.GetFileDictionary().OrderBy(pair => pair.Value).Reverse();

            foreach (var (key, value) in freq)
            {
                Console.WriteLine($"{key} : {value}");
            }

            var wordCloud = new WordCloudInput(freq.Select(p => new WordCloudEntry(p.Key, p.Value)))
            {
                Width       = 2560,
                Height      = 1440,
                MinFontSize = 6,
                MaxFontSize = 48
            };

            var sizer     = new LogSizer(wordCloud);
            var engine    = new SkGraphicEngine(sizer, wordCloud);
            var layout    = new SpiralLayout(wordCloud);
            var colorizer = new RandomColorizer(); // optional
            var wcg       = new WordCloudGenerator<SKBitmap>(wordCloud, engine, layout, colorizer);

            // Draw the bitmap on black background
            var final  = new SKBitmap(wordCloud.Width, wordCloud.Height);
            var canvas = new SKCanvas(final);

            canvas.Clear(SKColors.Black);
            canvas.DrawBitmap(wcg.Draw(), 0, 0);

            // Save to PNG
            var data          = final.Encode(SKEncodedImageFormat.Png, 100);
            var writer        = new FileInfo(options.OutputPathStr).Create();

            data.SaveTo(writer);
        }

        public static void Main(string[] args)
        {
            Program.ParseOptionsAndRun(args);
        }
    }
}
