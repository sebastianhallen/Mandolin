namespace Mandolin.Runners
{
    public class NUnitConsoleMandolinRunner
        : IMandolinRunner
    {
        private readonly INUnitConsoleFacade nunitConsole;
        private readonly NUnitConsoleCommandLineArgumentSlicer argumentSlicer;
        private readonly string[] args;

        public NUnitConsoleMandolinRunner(INUnitConsoleFacade nunitConsole, NUnitConsoleCommandLineArgumentSlicer argumentSlicer, params string[] args)
        {
            this.nunitConsole = nunitConsole;
            this.argumentSlicer = argumentSlicer;
            this.args = args;
        }

        public int Run(int wantedSlice, int totalSlices)
        {
            string[] slicedArgs;
            using (this.CreateArguments(wantedSlice, totalSlices, out slicedArgs))
            {
                 return this.nunitConsole.Run(slicedArgs);
            }
        }

        private IRunListFile CreateArguments(int wantedSlice, int totalSlices, out string[] slicedArgs)
        {
            return this.argumentSlicer.Slice(this.args, wantedSlice, totalSlices, out slicedArgs);
        }
    }
}