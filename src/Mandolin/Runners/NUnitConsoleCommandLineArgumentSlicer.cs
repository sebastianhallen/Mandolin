namespace Mandolin.Runners
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.ConsoleRunner;

    public class NUnitConsoleCommandLineArgumentSlicer
    {
        private readonly ISlicer slicer;
        private readonly ITestSuiteExtractor suiteExtractor;

        public NUnitConsoleCommandLineArgumentSlicer(ISlicer slicer, ITestSuiteExtractor suiteExtractor)
        {
            this.slicer = slicer;
            this.suiteExtractor = suiteExtractor;
        }

        public string[] Slice(string[] args, int wantedSlice, int totalSlices)
        {
            var options = new ConsoleOptions(args);
            var matchingTests = this.suiteExtractor.FindMatchingTests(options, options.Parameters.OfType<string>().ToArray());
            
            var slice = this.slicer.Slice(matchingTests, wantedSlice, totalSlices);

            return ReAssembleArguments(args, slice);
        }

        private static string[] ReAssembleArguments(string[] args, IEnumerable<string> slices)
        {
            var arguments = ToArgumentDictionary(args);
            var argumentsToRemove = new[] { "run", "runlist" }.SelectMany(arg => GetNamedArgument(arguments, arg));

            var runArgument = slices.ToArray().Any()
                                  ? new[] { new KeyValuePair<string, string>("/run", string.Join(",", slices)) }
                                  : new[] { new KeyValuePair<string, string>("/run", "NoMatchingTestsInSlice") };

            return arguments
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
            var parts = arg.Split(':', '=');

            if (parts.Count() == 2)
            {
                var value = parts[1];
                var key = parts[0];

                return new KeyValuePair<string, string>(key, value);
            }

            return new KeyValuePair<string, string>(parts[0], null);
        }
    }
}