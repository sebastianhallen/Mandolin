namespace Mandolin.Console
{
    using System.Configuration;
    using Mandolin.Slicers;
    using Mandolin.Slicers.Scorer;

    internal class SlicerFactory
    {
        public static ISlicer GetSlicer()
        {
            var wantedSlicerSetting = ConfigurationManager.AppSettings.Get("slicer");
            switch (wantedSlicerSetting)
            {
                case "ByPreviousTestRun":
                    var resultsFolder = ConfigurationManager.AppSettings.Get("PreviousTestRunResultsFolder");
                    var resultsRepository = new DirectoryScanningNUnitResultsRepository(resultsFolder);
                    var scorer = new NUnitResultXmlScorer(resultsRepository);
                    return new GreedyPartitionSlicer(scorer);
                case "SequentialGroups":
                    return new SequentialGroupsSlicer();
                case "Alternating": 
                default:
                    return new AlternatingSlicer();
            }
        }
    }
}