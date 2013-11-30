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
            var runner = new SimplifiedMandolinRunner(new AlternatingSlicer(), new MandolinEventListener());
            var testAssemblyPath = Path.GetFullPath(@"..\..\..\Example.Tests\bin\Debug\Example.Tests.dll");

            var result = runner.Run(2, 5, testAssemblyPath);

            Console.WriteLine(result);
        }
    }
}
