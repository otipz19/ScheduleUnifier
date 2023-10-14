namespace ScheduleUnifier.FileProviders
{
    public interface IFileProvider
    {
        public (string filePath, bool isExcel)[] GetFilesToParse();
    }
}
