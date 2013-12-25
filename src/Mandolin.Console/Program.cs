namespace Mandolin.Console
{
    using System.Collections.Generic;
    using Mandolin.Runners;
    using NUnit.Core;
    using Console = System.Console;

    class Program
    {
        static void Main(string[] args)
        {
            CoreExtensions.Host.InitializeService();

            List<string> cleanedArguments;
            int currentSliceNumber;
            int totalNumberOfSlices;
            ArgumentPreProcessor.Process(args, out currentSliceNumber, out totalNumberOfSlices, out cleanedArguments);

            var slicer = SlicerFactory.GetSlicer();
            var suiteExtractor = new TestSuiteExtractor();
            var runListBuilder = new DefaultRunListBuilder();
            var argumentSlicer = new NUnitConsoleCommandLineArgumentSlicer(slicer, suiteExtractor, runListBuilder);

            var nunitConsoleRunner = new NUnitConsoleMandolinRunner(new NUnitConsoleFacade(), argumentSlicer, cleanedArguments.ToArray());

            var result = nunitConsoleRunner.Run(currentSliceNumber, totalNumberOfSlices);

            Console.WriteLine(result);
        }
    }
}
