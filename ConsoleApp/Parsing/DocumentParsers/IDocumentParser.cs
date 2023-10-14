using ScheduleUnifier.Parsing.Models;

namespace ScheduleUnifier.Parsing.DocumentParsers
{
    public interface IDocumentParser 
    {
        public ParsedDocument Parse();
    }
}
