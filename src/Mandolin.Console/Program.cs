namespace Mandolin.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mandolin.Runners;
    using Mandolin.Slicers;
    using NUnit.Core;
    using Console = System.Console;

    class Program
    {
        static void Main(string[] args)
        {
            CoreExtensions.Host.InitializeService();

            var cleanedArguments = new List<string>();
            int currentSliceNumber = 0;
            int totalNumberOfSlices = 0;
            
            foreach (var arg in args)
            {
                int candidateSliceNumber;
                int candidateTotalNumberOfSlices;

                if (TryGetSlice(arg, out candidateSliceNumber, out candidateTotalNumberOfSlices))
                {
                    currentSliceNumber = candidateSliceNumber;
                    totalNumberOfSlices = candidateTotalNumberOfSlices;
                }
                else
                {
                    cleanedArguments.Add(arg);
                }
            }

            if (currentSliceNumber == 0 && totalNumberOfSlices == 0)
            {
                currentSliceNumber = 1;
                totalNumberOfSlices = 1;
            }
            
            var slicer = new AlternatingSlicer();
            var suiteExtractor = new TestSuiteExtractor();
            var argumentSlicer = new NUnitConsoleCommandLineArgumentSlicer(slicer, suiteExtractor);

            var nunitConsoleRunner = new NUnitConsoleMandolinRunner(new NUnitConsoleFacade(), argumentSlicer, cleanedArguments.ToArray());

            var result = nunitConsoleRunner.Run(currentSliceNumber, totalNumberOfSlices);

            Console.WriteLine(result);
        }

        private static bool TryGetSlice(string arg, out int currentSlice, out int totalSlices)
        {
            currentSlice = 1;
            totalSlices = 1;

            var parts = arg.Split(':', '=');
            if (parts.Count() != 2) return false;
            if (!parts[0].TrimStart('/', '-').Equals("slice", StringComparison.InvariantCultureIgnoreCase)) return false;

            return TryGetSliceValue(parts[1], out currentSlice, out totalSlices);
        }

        private static bool TryGetSliceValue(string value, out int currentSlice, out int totalSlices)
        {
            currentSlice = 1;
            totalSlices = 1;

            var parts = value.Split(new[] {"of"}, 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Count() != 2) return false;
            if (!int.TryParse(parts[0], out currentSlice)) return false;
            if (!int.TryParse(parts[1], out totalSlices)) return false;

            return currentSlice <= totalSlices;
        }
    }
}
