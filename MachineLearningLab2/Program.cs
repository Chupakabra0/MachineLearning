using FLS;
using FLS.Rules;

namespace MachineLearningLab2
{
    internal class Program
    { 
        internal class Options
        {

        }

        private static void ParseOptionsAndRun(string[] args)
        {
            Program.Run(null);
        }

        private static void Run(Options? options)
        {
            var temperature = new LinguisticVariable("Temperature");
            var cold        = temperature.MembershipFunctions.AddTriangle("Cold", -30, -15, 0);
            var norm        = temperature.MembershipFunctions.AddTriangle("Norm", -10, 10, 30);
            var hot         = temperature.MembershipFunctions.AddTriangle("Hot", 20, 30, 40);

            var pressure = new LinguisticVariable("Pressure");
            var small    = pressure.MembershipFunctions.AddTrapezoid("Small", 400, 400, 550, 750);
            var normal   = pressure.MembershipFunctions.AddTriangle("Normal", 730, 760, 780);
            var big      = pressure.MembershipFunctions.AddTrapezoid("Big", 770, 900, 1000, 1000);

            var sea      = new LinguisticVariable("Sea");
            var low      = temperature.MembershipFunctions.AddTriangle("Low", -500, 0, 500);
            var avarage  = temperature.MembershipFunctions.AddTriangle("Avarage", 400, 1000, 2000);
            var high     = temperature.MembershipFunctions.AddTriangle("High", 1500, 3000, 9000);

            var comfortLevel = new LinguisticVariable("Comfort Level");
            var happy        = comfortLevel.MembershipFunctions.AddRectangle("Happy", 70, 100);
            var ok           = comfortLevel.MembershipFunctions.AddRectangle("Ok", 45, 75);
            var sad          = comfortLevel.MembershipFunctions.AddRectangle("Sad", 25, 50);
            var fuck         = comfortLevel.MembershipFunctions.AddRectangle("Fuck", -1, 30);

            IFuzzyEngine fuzzyEngine = new FuzzyEngineFactory().Default();

            var ruleList = new List<FuzzyRule> {
                Rule.If(temperature.Is(norm).And(pressure.Is(normal)).And(sea.Is(avarage))).Then(comfortLevel.Is(happy)),
                Rule.If(temperature.Is(cold).And(pressure.Is(small)).And(sea.Is(high))).Then(comfortLevel.Is(ok)),
                Rule.If(temperature.Is(hot).And(pressure.Is(big)).And(sea.Is(low))).Then(comfortLevel.Is(ok)),
                Rule.If(temperature.Is(cold).And(pressure.IsNot(normal)).And(sea.Is(norm))).Then(comfortLevel.Is(sad)),
                Rule.If(temperature.IsNot(cold).And(pressure.IsNot(normal)).And(sea.Is(norm))).Then(comfortLevel.Is(fuck)),
                Rule.If(temperature.Is(cold).And(sea.Is(low))).Then(comfortLevel.Is(fuck)),
                Rule.If(temperature.Is(hot).And(sea.Is(high))).Then(comfortLevel.Is(fuck)),
                Rule.If(temperature.Is(cold).And(pressure.IsNot(small)).And(sea.Is(high))).Then(comfortLevel.Is(sad)),
                Rule.If(temperature.Is(hot).And(pressure.IsNot(big)).And(sea.Is(low))).Then(comfortLevel.Is(sad))
            };
            
            fuzzyEngine.Rules.Add(ruleList.ToArray());

            var result = fuzzyEngine.Defuzzify(new {
                temperature = 100,
                pressure    = 750,
                sea         = 800
            });

            Console.WriteLine($"Result: {result}");
        }

        public static void Main(string[] args)
        {
            Program.ParseOptionsAndRun(args);
        }
    }
}