namespace ScheduleUnifier.Exceptions
{
    public class MissingSavedTimeException : MissingSavedValueException
    {
        public MissingSavedTimeException(int row) : base(row, "TIME")
        {
        }
    }
}
