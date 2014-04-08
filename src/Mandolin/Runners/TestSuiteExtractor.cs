namespace Mandolin.Runners
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using NUnit.ConsoleRunner;
    using NUnit.Core;
    using NUnit.Core.Filters;
    using NUnit.Util;

    public interface ITestSuiteExtractor
    {
        IEnumerable<string> FindMatchingTests(ConsoleOptions options, params string[] testAssemblies);
    }

    public class TestSuiteExtractor
        : ITestSuiteExtractor
    {
        public IEnumerable<string> FindMatchingTests(ConsoleOptions options, params string[] testAssemblies)
        {
            TestFilter filter;
            if (!CreateTestFilter(options, out filter))
            {
                throw new Exception("Unable to create test filter");
            }

            var suite = CreateSuite(testAssemblies);
            var tests = GetTests(suite, new List<string>());
            
            foreach (var test in tests)
            {
                if (filter.Pass(test))
                    yield return test.TestName.FullName;
            }
        }

        private static IEnumerable<ITest> GetTests(ITest node, List<string> parentCategories)
        {
            if (node == null) yield break;
            if (node.Tests == null)
            {
                foreach (var category in parentCategories.Distinct())
                {
                    node.Categories.Add(category);
                }
                yield return node;
                yield break;
            }


            foreach (var childResult in node.Tests.OfType<ITest>())
            {
                var categories = new List<string>(parentCategories);

                categories.AddRange(node.Categories.OfType<string>());
                foreach (var test in GetTests(childResult, categories))
                {
                    yield return test;
                }
            }
        }

        private static TestSuite CreateSuite(string[] testAssemblies)
        {
            var fullAssemblyPaths = testAssemblies.Select(assembly =>
                {
                    if (Path.IsPathRooted(assembly))
                    {
                        return assembly;
                    }

                    return Path.Combine(AssemblyDirectory, assembly);
                });
            var package = new TestPackage("test", fullAssemblyPaths.ToArray());
            var builder = new TestSuiteBuilder();
            return builder.Build(package);
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        #region CreateTestFilter from NUnits ConsoleUi.cs
        internal static bool CreateTestFilter(ConsoleOptions options, out TestFilter testFilter)
        {
            testFilter = TestFilter.Empty;

            SimpleNameFilter nameFilter = new SimpleNameFilter();

            if (options.run != null && options.run != string.Empty)
            {
                Notify.WriteLine("Selected test(s): " + options.run);

                foreach (string name in TestNameParser.Parse(options.run))
                    nameFilter.Add(name);

                testFilter = nameFilter;
            }

            if (options.runlist != null && options.runlist != string.Empty)
            {
                Notify.WriteLine("Run list: " + options.runlist);

                try
                {
                    using (StreamReader rdr = new StreamReader(options.runlist))
                    {
                        // NOTE: We can't use rdr.EndOfStream because it's
                        // not present in .NET 1.x.
                        string line = rdr.ReadLine();
                        while (line != null && line.Length > 0)
                        {
                            if (line[0] != '#')
                                nameFilter.Add(line);
                            line = rdr.ReadLine();
                        }
                    }
                }
                catch (Exception e)
                {
                    if (e is FileNotFoundException || e is DirectoryNotFoundException)
                    {
                        Notify.WriteLine("Unable to locate file: " + options.runlist);
                        return false;
                    }
                    throw;
                }

                testFilter = nameFilter;
            }

            if (options.include != null && options.include != string.Empty)
            {
                TestFilter includeFilter = new CategoryExpression(options.include).Filter;
                Notify.WriteLine("Included categories: " + includeFilter.ToString());

                if (testFilter.IsEmpty)
                    testFilter = includeFilter;
                else
                    testFilter = new AndFilter(testFilter, includeFilter);
            }

            if (options.exclude != null && options.exclude != string.Empty)
            {
                TestFilter excludeFilter = new NotFilter(new CategoryExpression(options.exclude).Filter);
                Notify.WriteLine("Excluded categories: " + excludeFilter.ToString());

                if (testFilter.IsEmpty)
                    testFilter = excludeFilter;
                else if (testFilter is AndFilter)
                    ((AndFilter)testFilter).Add(excludeFilter);
                else
                    testFilter = new AndFilter(testFilter, excludeFilter);
            }

            if (testFilter is NotFilter)
                ((NotFilter)testFilter).TopLevel = true;

            return true;
        }
        #endregion

    }
}