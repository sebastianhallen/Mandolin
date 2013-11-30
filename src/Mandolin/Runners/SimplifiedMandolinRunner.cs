namespace Mandolin.Runners
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using NUnit.Core;
    using NUnit.Core.Filters;
    using NUnit.Util;

    public class SimplifiedMandolinRunner
        : IMandolinRunner
    {
        private readonly ISlicer slicer;
        private readonly EventListener eventListener;
        private readonly string[] testAssemblies;

        public SimplifiedMandolinRunner(ISlicer slicer, EventListener eventListener, params string[] testAssemblies)
        {
            this.slicer = slicer;
            this.eventListener = eventListener;
            this.testAssemblies = testAssemblies;
        }

        public string Run(int wantedSlice, int totalSlices)
        {
            CoreExtensions.Host.InitializeService();
            var runner = new SimpleTestRunner();

            var package = new TestPackage("tests", this.testAssemblies);

            if (runner.Load(package))
            {
                var allTests = GetFullTestNames(runner.Test);
                var slicedTests = this.slicer.Slice(allTests, wantedSlice, totalSlices);
                var filter = new SimpleNameFilter(slicedTests.ToArray());
                var result = runner.Run(this.eventListener, filter, true, LoggingThreshold.All);

                return CreateXmlOutput(result);
            }

            throw new Exception("Unable to load test package");
        }

        private static IEnumerable<string> GetFullTestNames(ITest parent)
        {
            if (parent == null) yield break;
            if (parent.Tests == null)
            {
                yield return parent.TestName.FullName;
                yield break;
            }


            foreach (var childResult in parent.Tests.OfType<ITest>())
            {
                foreach (var testName in GetFullTestNames(childResult))
                {
                    yield return testName;
                }
            }
        }

        private static string CreateXmlOutput(TestResult result)
        {
            var builder = new StringBuilder();
            new XmlResultWriter(new StringWriter(builder)).SaveTestResult(result);

            return builder.ToString();
        }
    }
}