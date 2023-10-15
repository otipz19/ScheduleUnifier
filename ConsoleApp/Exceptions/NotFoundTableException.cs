namespace ScheduleUnifier.Exceptions
{
    public class NotFoundTableException : ApplicationException
    {
        public NotFoundTableException(string? filePath)
            : base($"Not found table in file: {filePath}")
        {
        }

        public NotFoundTableException(string? filePath, Exception? innerException)
            : base($"Not found table in file: {filePath}", innerException)
        {
        }
    }
}
