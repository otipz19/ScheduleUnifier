namespace ScheduleUnifier.FileProviders
{
    internal class HttpFileLoader
    {
        public async Task LoadFile(string url, string localFilePathWithoutExtension)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var contentType = response.Content.Headers.ContentType;
                        if (contentType is not null)
                        {
                            string? mediaType = contentType.MediaType;

                            string? fileExtension = GetFileExtension(mediaType);

                            if (fileExtension is not null)
                            {
                                string localPath = localFilePathWithoutExtension + fileExtension;

                                byte[] content = await response.Content.ReadAsByteArrayAsync();

                                File.WriteAllBytes(localPath, content);
                            }
                            else
                            {
                                Console.WriteLine("Failed to determine the file extension.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Failed to determine content type.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Failed to download file. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        static string? GetFileExtension(string? contentType)
        {
            switch (contentType)
            {
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    return ".xlsx";
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    return ".docx";
                case "application/msword":
                    return ".doc";
                default:
                    return null;
            }
        }
    }
}
