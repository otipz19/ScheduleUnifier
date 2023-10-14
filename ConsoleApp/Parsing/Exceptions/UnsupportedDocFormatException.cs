namespace ScheduleUnifier.Parsing.Exceptions
{
    public class UnsupportedDocFormatException : Exception
    {
        public UnsupportedDocFormatException() 
            : base(
@"Binary .doc format is currently unsupported.
To continue parsing manually convert .doc to .docx using Microsoft Office Word.
Then reboot this application."
                  )
        {
        }
    }
}
