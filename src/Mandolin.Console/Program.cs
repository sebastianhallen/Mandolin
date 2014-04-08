namespace Mandolin.Console
{
    using System;
    using System.Collections.Generic;
    using Mandolin.Runners;
    using NUnit.Core;

    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);
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

            Notify.WriteLine("Exit code: " + result);
            Environment.Exit(result);
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var message = string.Format("(Mandolin) Unhandled exception: {0}", e.ExceptionObject);
            try
            {
                Notify.WriteLine(message);
            }
            finally
            {
                if (e.IsTerminating)
                {
                    Environment.FailFast(message);
                }
            }
        }
    }
}
