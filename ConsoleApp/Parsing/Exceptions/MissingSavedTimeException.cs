namespace ScheduleUnifier.Parsing.Exceptions
{
    public class MissingSavedTimeException : MissingSavedValueException
    {
        public MissingSavedTimeException(int row) : base(row, "TIME")
        {
        }
    }
}
