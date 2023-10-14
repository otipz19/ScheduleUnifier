namespace ScheduleUnifier.FileProviders
{
    public class HttpFileProvider : LocalFileProvider, IFileProvider
    {
        private readonly IEnumerable<string> urls;

        public HttpFileProvider(IEnumerable<string> urls)
        {
            this.urls = urls;
        }

        public override (string filePath, bool isExcel)[] GetFilesToParse()
        {
            var httpLoader = new HttpFileLoader();
            foreach (var url in urls)
            {
                string localPath = Path.Join(FullInputPath, new Guid().ToString());
                httpLoader.LoadFile(url, localPath).GetAwaiter().GetResult();
            }
            return base.GetFilesToParse();
        }
    }
}
