namespace Mandolin.Runners
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.ConsoleRunner;

    public class NUnitConsoleCommandLineArgumentSlicer
    {
        private readonly ISlicer slicer;

        public NUnitConsoleCommandLineArgumentSlicer(ISlicer slicer)
        {
            this.slicer = slicer;
        }

        public string[] Slice(string[] args, int wantedSlice, int totalSlices)
        {
            var arguments = ToArgumentDictionary(args);

            var runArguments = GetNamedArgument(arguments, "run");
            if (runArguments.Any())
            {
                //use existing run argument
                var run = runArguments.First().Value;
                var slice = this.slicer.Slice(run.Split(','), wantedSlice, totalSlices);

                return ReAssembleArguments(arguments.Except(runArguments), slice);
            }


            return args;
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
                                .ToDictionary(arg => arg.Key.ToLowerInvariant(), arg => arg.Value);
            return arguments;
        }

        private static KeyValuePair<string, string> ToArgumentPair(string arg)
        {
            var parts = arg.Split(':');

            if (parts.Count() == 2)
            {
                var value = parts[1];
                var key = parts[0];

                return new KeyValuePair<string, string>(key, value);
            }

            return new KeyValuePair<string, string>(parts[0], null);
        }

        private static string[] ReAssembleArguments(IEnumerable<KeyValuePair<string, string>> arguments, IEnumerable<string> slices)
        {
            return arguments
                .Select(kvp => kvp.Value == null 
                    ? kvp.Key
                    : string.Format(@"--{0}:""{1}""", kvp.Key, kvp.Value))
                .Concat(new [] { "--run:" + string.Join(",", slices)})
                .ToArray();
        }
    }
}