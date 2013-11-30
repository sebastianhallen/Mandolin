namespace Mandolin
{
    public interface IMandolinRunner
    {
        string Run(int wantedSlice, int totalSlices, params string[] testAssemblies);
    }
}