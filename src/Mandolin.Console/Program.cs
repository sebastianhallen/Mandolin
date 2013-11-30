namespace Mandolin.Console
{
    using Mandolin.Runners;
    using Mandolin.Slicers;
    using System.IO;
    using Console = System.Console;

    class Program
    {
        static void Main(string[] args)
        {
            //var args = "/nothread /noshadow /labels /timeout:10000 /exclude:DontRun,NeverRun /include:MustRun,ShouldRun /xml="NUnitLog.xml" ..\..\..\Example.Tests\bin\Debug\Example.Tests.dll".Split(' '); 

            var testAssemblyPath = Path.GetFullPath(@"..\..\..\Example.Tests\bin\Debug\Example.Tests.dll");
            var simpleRunner = new SimplifiedMandolinRunner(new AlternatingSlicer(), testAssemblyPath);
            var nunitConsoleRunner = new NUnitConsoleMandolinRunner(new NUnitConsoleFacade(), new AlternatingSlicer(), args);

            var result = simpleRunner.Run(2, 5);
            nunitConsoleRunner.Run(3, 4);

            Console.WriteLine(result);
        }
    }
}
