namespace Mandolin.Console
{
    using Mandolin.Runners;
    using Mandolin.Slicers;
    using System.IO;
    using NUnit.Core;
    using Console = System.Console;

    class Program
    {
        static void Main(string[] args)
        {
            CoreExtensions.Host.InitializeService();
            //var args = "/nothread /noshadow /labels /timeout:10000 /exclude:DontRun,NeverRun /include:MustRun,ShouldRun /xml="NUnitLog.xml" ..\..\..\Example.Tests\bin\Debug\Example.Tests.dll".Split(' '); 

            //var testAssemblyPath = Path.GetFullPath(@"..\..\..\Example.Tests\bin\Debug\Example.Tests.dll");
            //var simpleRunner = new SimplifiedMandolinRunner(new AlternatingSlicer(), testAssemblyPath);
            var slicer = new AlternatingSlicer();
            var suiteExtractor = new TestSuiteExtractor();
            var argumentSlicer = new NUnitConsoleCommandLineArgumentSlicer(slicer, suiteExtractor);

            var nunitConsoleRunner = new NUnitConsoleMandolinRunner(new NUnitConsoleFacade(), argumentSlicer, args);

            //var result = simpleRunner.Run(2, 5);
            var result = nunitConsoleRunner.Run(3, 4);

            Console.WriteLine(result);
        }
    }
}
