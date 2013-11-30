namespace Mandolin.Console
{
    using Mandolin.Slicers;
    using NUnit.Core;
    using NUnit.Util;
    using System.IO;
    using System.Text;
    using Console = System.Console;

    class Program
    {
        static void Main(string[] args)
        {
            var runner = new MandolinRunner(new AlternatingSlicer(), new MandolinEventListener());
            var testAssemblyPath = Path.GetFullPath(@"..\..\..\Example.Tests\bin\Debug\Example.Tests.dll");
            var result = runner.Run(2, 5, testAssemblyPath);

            var output = CreateXmlOutput(result);
            Console.WriteLine(output);
        }

        private static string CreateXmlOutput(TestResult result)
        {
            StringBuilder builder = new StringBuilder();
            new XmlResultWriter(new StringWriter(builder)).SaveTestResult(result);

            return builder.ToString();
        }
    }
}
