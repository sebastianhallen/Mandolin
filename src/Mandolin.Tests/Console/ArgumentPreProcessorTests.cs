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
        //this is for when you cannot supply additional parameters
        //hack? You bet!
        public void Should_be_able_to_sneak_in_an_assembly_name_as_a_magic_string_to_configure_slicing()
        {
            int currentSliceNumber;
            int totalNumberOfSlices;
            List<string> transformedArguments;

            ArgumentPreProcessor.Process(new [] { "some.dll,slice-with-mandolin--slice-1of2"}, out currentSliceNumber, out totalNumberOfSlices, out transformedArguments);

            Assert.That(currentSliceNumber, Is.EqualTo(1));
            Assert.That(totalNumberOfSlices, Is.EqualTo(2));
            Assert.That(transformedArguments.Single(), Is.EqualTo("some.dll"));
        }

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
