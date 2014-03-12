namespace Mandolin.Tests.Console
{
    using NUnit.Framework;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    [TestFixture, Explicit]
    public class ConsoleIntegrationTests
    {
        [Test]
        [Category("NeverEverRun")]
        public void Should_be_able_to_run_mandolin_tests()
        {
            var info = new ProcessStartInfo();
            info.FileName = "Mandolin.Console.exe";
            info.Arguments = "Mandolin.Tests.dll /exclude:NeverEverRun /include:crash";

            var process = Process.Start(info);
            var exited = process.WaitForExit(10000);
        }
    }

    [TestFixture, Explicit]
    [Category("crash")]
    public class CrashInTest
    {
        [Test]
        public void CrashesInTest()
        {
            using (new CrashesInDispose()) Console.WriteLine("This is going to hurt...");
        }
    }

    [TestFixture, Explicit]
    [Category("crash")]
    public class CrashInSetup
    {
        [SetUp]
        public void Crash()
        {
            using (new CrashesInDispose()) Console.WriteLine("This is going to hurt...");
        }

        [Test]
        public void Dummy() { }
    }

    [TestFixture, Explicit]
    [Category("crash")]
    public class CrashInTearDown
    {
        [TearDown]
        public void Crash()
        {
            using (new CrashesInDispose()) Console.WriteLine("This is going to hurt...");
        }

        [Test]
        public void Dummy() { }
    }

    internal class CrashesInDispose
        : IDisposable
    {
        public void Dispose()
        {
            Parallel.For(0, 10, (i, state) => { throw new Exception(); });
        }
    }
}
