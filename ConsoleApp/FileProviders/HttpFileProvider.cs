namespace ScheduleUnifier.FileProviders
{
    public class HttpFileProvider : LocalFileProvider, IFileProvider
    {
        private readonly IEnumerable<string> urls;

        public HttpFileProvider(IEnumerable<string> urls)
        {
            this.urls = urls;
        }

        public override (string filePath, bool isExcel)[] GetFilesToParse(string inputDirPath)
        {
            if(!Directory.Exists(inputDirPath))
            {
                Directory.CreateDirectory(inputDirPath);
            }
            else
            {
                foreach (var file in Directory.EnumerateFiles(inputDirPath))
                {
                    File.Delete(file);
                }
            }

            var httpLoader = new HttpFileLoader();
            foreach (var url in urls)
            {
                string localPath = Path.Join(inputDirPath, Guid.NewGuid().ToString());
                httpLoader.LoadFile(url, localPath).GetAwaiter().GetResult();
            }
            return base.GetFilesToParse(inputDirPath);
        }
    }
}
