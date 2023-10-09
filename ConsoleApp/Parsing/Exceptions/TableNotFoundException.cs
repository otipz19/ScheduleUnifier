namespace ConsoleApp.Parsing.Exceptions
{
    internal class TableNotFoundException : ApplicationException
    {
        public TableNotFoundException(string? filePath, Exception? innerException) 
            : base($"Not found table in file: {filePath}", innerException)
        {
        }
    }
}
