namespace Mandolin.Runners
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NUnit.ConsoleRunner;

    public class NUnitConsoleCommandLineArgumentSlicer
    {
        private readonly ISlicer slicer;
        private readonly ITestSuiteExtractor suiteExtractor;
	    private readonly IRunListFilter preFilter;
        private readonly IRunListBuilder runListBuilder;

		public NUnitConsoleCommandLineArgumentSlicer(ISlicer slicer, ITestSuiteExtractor suiteExtractor, IRunListFilter preFilter, IRunListBuilder runListBuilder)
        {
            this.slicer = slicer;
            this.suiteExtractor = suiteExtractor;
			this.preFilter = preFilter;
            this.runListBuilder = runListBuilder;
        }

        public IRunListFile Slice(string[] args, int wantedSlice, int totalSlices, out string[] slicedArgs)
        {
            var options = new ConsoleOptions(args);
            var matchingTests = this.suiteExtractor.FindMatchingTests(options, options.Parameters.OfType<string>().ToArray());
	        var filteredTests = this.preFilter.Filter(matchingTests);

			var slice = this.slicer.Slice(filteredTests, wantedSlice, totalSlices);

            return this.ReAssembleArguments(args, slice, out slicedArgs);
        }

        private IRunListFile ReAssembleArguments(string[] args, IEnumerable<string> slices, out string[] slicedArgs)
        {
            var arguments = ToArgumentDictionary(args);
            var argumentsToRemove = new[] { "run", "runlist" }.SelectMany(arg => GetNamedArgument(arguments, arg));

            var runlist = this.runListBuilder.CreateRunList(slices);
            var runArgument = slices.ToArray().Any()
                                  ? new[] { new KeyValuePair<string, string>("/runlist", runlist.Path) }
                                  : new[] { new KeyValuePair<string, string>("/run", "NoMatchingTestsInSlice") };

            slicedArgs = arguments
                .Except(argumentsToRemove)
                .Concat(runArgument)
                .OrderBy(kvp => kvp.Value == null)
                .ThenByDescending(kvp => kvp.Key.StartsWith("-") || kvp.Key.StartsWith("/"))
                .Select(kvp =>
                {
                    if (kvp.Value == null) return kvp.Key;
                    var value = kvp.Value;
                    value = value.Contains(" ")
                                ? @"""" + value + @""""
                                : value;
                    return string.Format(@"{0}:{1}", kvp.Key, value);
                })
                .ToArray();

            return runlist;
        }

        private static KeyValuePair<string, string>[] GetNamedArgument(Dictionary<string, string> allArguments, string namedArgument)
        {
            return
                allArguments.Where(
                    arg =>
                    arg.Key.TrimStart('-', '/').Equals(namedArgument, StringComparison.InvariantCultureIgnoreCase)
                ).ToArray();

        }

        private static Dictionary<string, string> ToArgumentDictionary(string[] args)
        {
            var arguments = args.Select(ToArgumentPair)
                                .ToDictionary(arg => arg.Key, arg => arg.Value);
            return arguments;
        }

        private static KeyValuePair<string, string> ToArgumentPair(string arg)
        {
			if (File.Exists(arg))
			{
				return new KeyValuePair<string, string>(arg, null);
			}

            var colonIndex = arg.IndexOf(':');
            var equalsIndex = arg.IndexOf('=');

            if (colonIndex == -1) colonIndex = int.MaxValue;
            if (equalsIndex == -1) equalsIndex = int.MaxValue;
            var delimiter = colonIndex > equalsIndex ? "=" : ":";

            var parts = arg.Split(new[] {delimiter}, StringSplitOptions.None);

            if (parts.Count() >= 2)
            {
                var value = string.Join(delimiter, parts.Skip(1));
                var key = parts[0];

                return new KeyValuePair<string, string>(key, value);
            }

            return new KeyValuePair<string, string>(arg, null);
        }
    }
}