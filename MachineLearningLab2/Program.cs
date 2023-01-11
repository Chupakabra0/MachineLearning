using CommandLine;
using FLS;
using FLS.Rules;

namespace MachineLearningLab2 {
    internal class Program {
        internal class TestOptions {
            [Option('t', "test", SetName = "app", HelpText = "Flag, that launch test cases")]
            public bool IsTest { get; set; }
        }

        internal class ProgramOptions {
            [Option('c', "cpu", Required = true, HelpText = "CPU performance rate [0; 100]")]
            public double CPU { get; set; }

            [Option('g', "gpu", Required = true, HelpText = "GPU performance rate [0; 100]")]
            public double GPU { get; set; }

            [Option('r', "ram", Required = true, HelpText = "RAM performance rate [0; 100]")]
            public double RAM { get; set; }
        }

        public static void Main(string[] args) {
            Program.ParseOptionsAndRun(args);
        }

        private static IFuzzyEngine CreateFuzzyEngine() {
            var fuzzyEngine = new FuzzyEngineFactory().Default();

            var centralProcessorLingusticVar = new LinguisticVariable("CPU");
            var centralProcessorBad          = centralProcessorLingusticVar.MembershipFunctions.AddTriangle("Bad CPU", 0, 7.5, 14);
            var centralProcessorGood         = centralProcessorLingusticVar.MembershipFunctions.AddTriangle("Good CPU", 10, 15, 20);
            var centralProcessorPerfect      = centralProcessorLingusticVar.MembershipFunctions.AddTriangle("Perfect CPU", 17.5, 40, 100);

            var videoCardLingusticVar = new LinguisticVariable("GPU");
            var videoCardBad          = videoCardLingusticVar.MembershipFunctions.AddTriangle("Bad GPU", 0, 10, 20);
            var videoCardGood         = videoCardLingusticVar.MembershipFunctions.AddTriangle("Good GPU", 15, 35, 55);
            var videoCardPerfect      = videoCardLingusticVar.MembershipFunctions.AddTriangle("Perfect GPU", 40, 100, 100);

            var memoryLingusticVar = new LinguisticVariable("RAM");
            var memoryBad          = memoryLingusticVar.MembershipFunctions.AddTriangle("Bad RAM", 0, 12.5, 30);
            var memoryGood         = memoryLingusticVar.MembershipFunctions.AddTriangle("Good RAM", 25, 40, 55);
            var memoryPerfect      = memoryLingusticVar.MembershipFunctions.AddTriangle("Perfect RAM", 45, 100, 100);

            var computerSpeedLingusticVar = new LinguisticVariable("PC Speed");
            var computerSpeedBad          = computerSpeedLingusticVar.MembershipFunctions.AddTrapezoid("Bad PC Speed", 0, 0, 15, 30);
            var computerSpeedGood         = computerSpeedLingusticVar.MembershipFunctions.AddTriangle("Good PC Speed", 20, 50, 75);
            var computerSpeedPerfect      = computerSpeedLingusticVar.MembershipFunctions.AddTrapezoid("Perfect PC Speed", 65, 85, 100, 100);

            var ruleList = new List<FuzzyRule> {
                // slow CPU cases
                Rule.If (
                    centralProcessorLingusticVar.Is(centralProcessorBad)
                        .And(memoryLingusticVar.Is(memoryBad))
                ).Then(computerSpeedLingusticVar.Is(computerSpeedBad)),
                Rule.If (
                    centralProcessorLingusticVar.Is(centralProcessorBad)
                        .And(videoCardLingusticVar.Is(videoCardBad))
                ).Then(computerSpeedLingusticVar.Is(computerSpeedBad)),
                Rule.If (
                    centralProcessorLingusticVar.Is(centralProcessorBad)
                        .And(videoCardLingusticVar.Is(videoCardBad))
                ).Then(computerSpeedLingusticVar.Is(computerSpeedBad)),
                Rule.If (
                    centralProcessorLingusticVar.Is(centralProcessorBad)
                        .And(memoryLingusticVar.Is(memoryPerfect))
                        .And(videoCardLingusticVar.IsNot(videoCardBad))
                ).Then(computerSpeedLingusticVar.Is(computerSpeedGood)),
                Rule.If (
                    centralProcessorLingusticVar.Is(centralProcessorBad)
                        .And(memoryLingusticVar.Is(memoryGood))
                        .And(videoCardLingusticVar.Is(videoCardPerfect))
                ).Then(computerSpeedLingusticVar.Is(computerSpeedGood)),
                Rule.If (
                    centralProcessorLingusticVar.Is(centralProcessorBad)
                        .And(memoryLingusticVar.Is(memoryGood))
                        .And(videoCardLingusticVar.Is(videoCardGood))
                ).Then(computerSpeedLingusticVar.Is(computerSpeedBad)),

                // good CPU cases
                Rule.If (
                    centralProcessorLingusticVar.Is(centralProcessorGood)
                        .And(memoryLingusticVar.Is(memoryPerfect))
                        .And(videoCardLingusticVar.Is(videoCardPerfect))
                ).Then(computerSpeedLingusticVar.Is(computerSpeedPerfect)),
                Rule.If (
                    centralProcessorLingusticVar.Is(centralProcessorGood)
                        .And(memoryLingusticVar.IsNot(memoryPerfect))
                        .And(videoCardLingusticVar.Is(videoCardPerfect))
                ).Then(computerSpeedLingusticVar.Is(computerSpeedGood)),
                Rule.If (
                    centralProcessorLingusticVar.Is(centralProcessorGood)
                        .And(memoryLingusticVar.Is(memoryPerfect))
                        .And(videoCardLingusticVar.IsNot(videoCardPerfect))
                ).Then(computerSpeedLingusticVar.Is(computerSpeedGood)),
                Rule.If (
                    centralProcessorLingusticVar.Is(centralProcessorGood)
                        .And(memoryLingusticVar.Is(memoryBad))
                        .And(videoCardLingusticVar.IsNot(videoCardPerfect))
                ).Then(computerSpeedLingusticVar.Is(computerSpeedBad)),
                Rule.If (
                    centralProcessorLingusticVar.Is(centralProcessorGood)
                        .And(memoryLingusticVar.Is(memoryGood))
                        .And(videoCardLingusticVar.Is(videoCardGood))
                ).Then(computerSpeedLingusticVar.Is(computerSpeedGood)),
                Rule.If (
                    centralProcessorLingusticVar.Is(centralProcessorGood)
                        .And(memoryLingusticVar.Is(memoryBad))
                        .And(videoCardLingusticVar.Is(videoCardBad))
                ).Then(computerSpeedLingusticVar.Is(computerSpeedBad)),

                // perfect CPU cases
                Rule.If (
                    centralProcessorLingusticVar.Is(centralProcessorPerfect)
                        .And(memoryLingusticVar.Is(memoryGood))
                ).Then(computerSpeedLingusticVar.Is(computerSpeedGood)),
                Rule.If (
                    centralProcessorLingusticVar.Is(centralProcessorPerfect)
                        .And(videoCardLingusticVar.Is(videoCardGood))
                ).Then(computerSpeedLingusticVar.Is(computerSpeedGood)),
                Rule.If (
                    centralProcessorLingusticVar.Is(centralProcessorPerfect)
                        .And(memoryLingusticVar.Is(memoryPerfect))
                        .And(videoCardLingusticVar.IsNot(videoCardBad))
                ).Then(computerSpeedLingusticVar.Is(computerSpeedPerfect)),
                Rule.If (
                    centralProcessorLingusticVar.Is(centralProcessorPerfect)
                        .And(memoryLingusticVar.Is(memoryGood))
                        .And(videoCardLingusticVar.Is(videoCardGood))
                ).Then(computerSpeedLingusticVar.Is(computerSpeedGood)),
                Rule.If (
                    centralProcessorLingusticVar.Is(centralProcessorPerfect)
                        .And(memoryLingusticVar.Is(memoryGood))
                        .And(videoCardLingusticVar.Is(videoCardPerfect))
                ).Then(computerSpeedLingusticVar.Is(computerSpeedPerfect))
            };

            fuzzyEngine.Rules.Add(ruleList.ToArray());

            return fuzzyEngine;
        }

        private static void ParseOptionsAndRun(string[] args) {
            var parser      = new Parser(with => with.HelpWriter = null);
            var testOptions = parser.ParseArguments<TestOptions>(args).Value;
            
            if (testOptions?.IsTest ?? false) {
                Program.TestRun();
            }
            else {
                var programOptions = Parser.Default.ParseArguments<ProgramOptions>(args).WithParsed(ArgRun);
            }
        }

        private static void PrintResult(string preString, double result) {
            Console.WriteLine(preString);

            if (result < 20.0) {
                Console.WriteLine($"Stupid dirty PC... Rate: {result}");
            }
            else if (result < 30.0) {
                Console.WriteLine($"Slow PC :( Rate: {result}");
            }
            else if (result < 65.0) {
                Console.WriteLine($"Good PC :) Rate: {result}");
            }
            else {
                Console.WriteLine($"Damn son where'd you find this?! Rate: {result}");
            }
        }

        private static void TestRun() {
            var result1 = Program.fuzzyEngine.Defuzzify(new {
                CPU = 13.13,
                GPU = 17.80,
                RAM = 30.00
            });

            PrintResult("Ryzen 5 1600 AF + RX 570 + 16 Gb DDR4 2933 MHz CL20", result1);

            var result2 = Program.fuzzyEngine.Defuzzify(new {
                CPU = 19.69,
                GPU = 42.36,
                RAM = 75.00
            });

            PrintResult("Ryzen 5 5600G + RTX 2060 + 16 DDR4 3600 MHz CL16", result2);

            var result3 = Program.fuzzyEngine.Defuzzify(new {
                CPU = 37.91,
                GPU = 68.38,
                RAM = 95.00
            });

            PrintResult("Intel i5-13600KF + RTX 3080 12GB + 32 DDR5 6000 MHz CL36", result3);
        }

        private static void ArgRun(ProgramOptions options) {
            double cpu = options.CPU;
            double gpu = options.GPU;
            double ram = options.RAM;

            var result = Program.fuzzyEngine.Defuzzify(new {
                CPU = cpu,
                GPU = gpu,
                RAM = ram
            });

            PrintResult($"CPU: {cpu}, GPU: {gpu}, RAM: {ram}", result);
        }

        private static IFuzzyEngine fuzzyEngine = Program.CreateFuzzyEngine();
    }
}