namespace ScheduleUnifier.Exceptions
{
    public class MissingSavedDayException : MissingSavedValueException
    {
        public MissingSavedDayException(int row) : base(row, "DAY")
        {
        }
    }
}
