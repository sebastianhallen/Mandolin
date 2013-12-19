namespace Mandolin.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ArgumentPreProcessor
    {
        public static void Process(string[] args, out int currentSliceNumber, out int totalNumberOfSlices, out List<string> transformedArguments)
        {
            currentSliceNumber = 0;
            totalNumberOfSlices = 0;
            transformedArguments = new List<string>();
            foreach (var arg in args)
            {
                int candidateSliceNumber;
                int candidateTotalNumberOfSlices;

                if (TryGetSlice(arg, out candidateSliceNumber, out candidateTotalNumberOfSlices))
                {
                    currentSliceNumber = candidateSliceNumber;
                    totalNumberOfSlices = candidateTotalNumberOfSlices;
                }
                else
                {
                    string assembly = null;
                    if (TryGetSliceFromMagicStringNamedAssembly(arg, out candidateSliceNumber, out candidateTotalNumberOfSlices, out assembly))
                    {
                        currentSliceNumber = candidateSliceNumber;
                        totalNumberOfSlices = candidateTotalNumberOfSlices;
                        transformedArguments.Add(assembly);
                    }
                    else
                    {
                        transformedArguments.Add(arg);
                    }
                }
            }

            if (currentSliceNumber == 0 && totalNumberOfSlices == 0)
            {
                currentSliceNumber = 1;
                totalNumberOfSlices = 1;
            }
        }

        private static bool TryGetSliceFromMagicStringNamedAssembly(string arg, out int candidateSliceNumber, out int candidateTotalNumberOfSlices, out string assembly)
        {
            candidateSliceNumber = 0;
            candidateTotalNumberOfSlices = 0;
            assembly = null;

            const string magicStringPrefix = "slice-with-mandolin--slice-";
            var assemblies = arg.Split(',');
            var magicStringAssemblies = assemblies
                                            .Where(asm => asm.Contains(magicStringPrefix))
                                            .Select(asm => asm.Replace(magicStringPrefix, ""))
                                            .ToArray();

            var slice = magicStringAssemblies.FirstOrDefault();

            if (slice != null && TryGetSlice("/slice:" + slice, out candidateSliceNumber, out candidateTotalNumberOfSlices))
            {
                assembly = string.Join(",", assemblies.Where(asm => !asm.Contains(magicStringPrefix)));
                return true;
            }

            return false;
        }

        private static bool TryGetSlice(string arg, out int currentSlice, out int totalSlices)
        {
            currentSlice = 1;
            totalSlices = 1;

            var parts = arg.Split(':', '=');
            if (parts.Count() != 2) return false;
            if (!parts[0].TrimStart('/', '-').Equals("slice", StringComparison.InvariantCultureIgnoreCase)) return false;

            return TryGetSliceValue(parts[1], out currentSlice, out totalSlices);
        }

        private static bool TryGetSliceValue(string value, out int currentSlice, out int totalSlices)
        {
            currentSlice = 1;
            totalSlices = 1;

            var parts = value.Split(new[] { "of" }, 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Count() != 2) return false;
            if (!int.TryParse(parts[0], out currentSlice)) return false;
            if (!int.TryParse(parts[1], out totalSlices)) return false;

            return currentSlice <= totalSlices;
        }
    }
}