﻿namespace Mandolin.Console
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

			var result = -1;
			using (var bootstrap = new Bootstrap())
			{
				var slicer = bootstrap.Resolve<ISlicer>();
				var suiteExtractor = bootstrap.Resolve<ITestSuiteExtractor>();
				var preFilter = bootstrap.Resolve<IRunListFilter>();
				var runListBuilder = bootstrap.Resolve<IRunListBuilder>();
				
				var argumentSlicer = new NUnitConsoleCommandLineArgumentSlicer(slicer, suiteExtractor, preFilter, runListBuilder);
				var nunitConsoleRunner = new NUnitConsoleMandolinRunner(new NUnitConsoleFacade(), argumentSlicer, cleanedArguments.ToArray());

				result = nunitConsoleRunner.Run(currentSliceNumber, totalNumberOfSlices);
	        }
	        
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
