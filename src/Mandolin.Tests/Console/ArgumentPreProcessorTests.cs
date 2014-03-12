namespace Mandolin.Tests.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Mandolin.Console;
    using NUnit.Framework;

    [TestFixture]
    public class ArgumentPreProcessorTests
    {
        [Test]
        public void Should_not_explode_when_not_supplying_magic_slice_string()
        {
            int currentSliceNumber;
            int totalNumberOfSlices;
            List<string> transformedArguments;

            ArgumentPreProcessor.Process(new[] { "some.dll" }, out currentSliceNumber, out totalNumberOfSlices, out transformedArguments);

            Assert.That(currentSliceNumber, Is.EqualTo(1));
            Assert.That(totalNumberOfSlices, Is.EqualTo(1));
            Assert.That(transformedArguments.Single(), Is.EqualTo("some.dll"));
        }
    }
}
