namespace ConsoleApp.Parsing.Exceptions
{
    public abstract class MissingSavedValueException : Exception
    {
        protected MissingSavedValueException(int row, string valueType) : base($"Missing saved {valueType} at {row} table row!")
        {
        }
    }
}
