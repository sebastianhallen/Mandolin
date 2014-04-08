namespace Mandolin.Runners
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public interface IRunListBuilder
    {
        IRunListFile CreateRunList(IEnumerable<string> testMethods);
    }

    public interface IRunListFile
        : IDisposable
    {
        string Path { get; }
    }

    public class DefaultRunListBuilder
        : IRunListBuilder
    {
        public IRunListFile CreateRunList(IEnumerable<string> testMethods)
        {
            return new DefaultRunListFile(this.CreateTempFilePath(), testMethods);
        }

        private string CreateTempFilePath()
        {
            var tempDir = Path.GetTempPath();
            var tempFile = Path.GetTempFileName();
            return Path.Combine(tempDir, tempFile);
        }
    }

    public class DefaultRunListFile
        : IRunListFile
    {
        public DefaultRunListFile(string tempFilePath, IEnumerable<string> testMethods)
        {
            this.Path = tempFilePath;
            this.PopulateRunFile(testMethods);
        }

        public void Dispose()
        {
            try
            {
                File.Delete(this.Path);
            }
            catch (Exception ex)
            {
                Notify.WriteLine("Unable to delete run list file: " + this.Path);
                Notify.WriteLine(ex.ToString());
            }
        }

        public string Path { get; private set; }

        private void PopulateRunFile(IEnumerable<string> testMethods)
        {
            var content = string.Join(Environment.NewLine, testMethods);
            Notify.WriteLine("Run list: {0}{1}", Environment.NewLine, content);
            File.WriteAllText(this.Path, content);
        }
    }
}
