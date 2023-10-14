namespace ConsoleApp.Parsing.Exceptions
{
    internal class NotFoundHeaderException : InvalidTableFormatException
    {
        public NotFoundHeaderException() : base("Table missing valid header!")
        {
        }
    }
}
