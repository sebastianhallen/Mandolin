namespace Mandolin.Slicers.Scorer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    public class DirectoryScanningNUnitResultsRepository
        : INUnitResultsRepository
    {
        private readonly string path;

        public DirectoryScanningNUnitResultsRepository(string path)
        {
            this.path = path;
        }

        public IEnumerable<INUnitResult> All()
        {
            return from file in Directory.GetFiles(path, "*.xml")
                        let xml = ParseXml(file)
                        from testCase in xml.Descendants(XName.Get("test-case"))
                        let name = testCase.Attribute("name").Value
                        let time = testCase.Attribute("time")
                        where time != null
                        let duration = time.Value
                        let durationMilliseconds = ToMilliseconds(duration)
                        where durationMilliseconds > 0
                        select new NUnitResult(TimeSpan.FromMilliseconds(durationMilliseconds), name);
        }

        private int ToMilliseconds(string duration)
        {
            int milliseconds;
            return int.TryParse(duration.Replace(".", ""), out milliseconds) 
                ? milliseconds 
                : 0;
        }

        private XDocument ParseXml(string filePath)
        {
            try
            {
                return XDocument.Load(filePath);
            }
            catch (Exception)
            {
                return XDocument.Parse("");
            }

        }
    }
}