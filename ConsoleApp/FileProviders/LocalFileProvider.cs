using ScheduleUnifier.Parsing.Exceptions;

namespace ScheduleUnifier.FileProviders
{
    public class LocalFileProvider : IFileProvider
    {
        protected string FullInputPath => Path.Join(AppDomain.CurrentDomain.BaseDirectory, "input");

        public virtual (string filePath, bool isExcel)[] GetFilesToParse()
        {
            var inputDir = new DirectoryInfo(FullInputPath);

            if (!inputDir.Exists)
            {
                throw new InputFilesNotFoundException();
            }

            string[] parsableExtensions = new string[] { ".doc", ".docx", ".xlsx" };
            var filesToParse = inputDir.GetFiles()
                .Where(f => parsableExtensions.Contains(f.Extension));

            if (!filesToParse.Any())
            {
                throw new InputFilesNotFoundException();
            }

            if (filesToParse.Any(f => f.Extension.Equals(".doc")))
            {
                throw new UnsupportedDocFormatException();
            }

            return filesToParse.Select(f => (f.FullName, f.Extension.Equals(".xlsx"))).ToArray();
        }
    }
}
