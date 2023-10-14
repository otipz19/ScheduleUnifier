using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ScheduleUnifier.Parsing.Exceptions;

namespace ScheduleUnifier.Parsing.FacultyAndSpecializationParsers
{
    internal class DocxFacultyAndSpecializationParser : IFacultyAndSpecializationParser
    {
        private readonly WordprocessingDocument document;
        private readonly FacultyFinder facultyFinder = new FacultyFinder();
        private readonly SpecializationsFinder specializationsFinder = new SpecializationsFinder();

        public DocxFacultyAndSpecializationParser(WordprocessingDocument document)
        {
            this.document = document;
        }

        public (string faculty, IEnumerable<string> specializations) Parse()
        {
            string? faculty = null;
            IEnumerable<string>? specializations = null;

            var paragraphs = document.MainDocumentPart!.Document.Body!.Elements<Paragraph>();

            foreach (Paragraph paragraph in paragraphs)
            {
                if (specializations is null || !specializations.Any())
                {
                    specializations = FindSpecializations(paragraph);
                }
                else
                {
                    break;
                }
            }

            var runs = paragraphs.SelectMany(p => p.Elements<Run>());

            foreach (Run run in runs)
            {
                bool hasFound = false;
                foreach (Text text in run.Elements<Text>())
                {
                    hasFound = facultyFinder.TryFind(text.Text, out faculty);

                    if (hasFound)
                        break;
                }

                if (hasFound)
                    break;
            }

            if (faculty is null)
            {
                throw new NotFoundFacultyException();
            }

            if (specializations is null || !specializations.Any())
            {
                throw new NotFoundSpecializationsException();
            }

            return (faculty, specializations);
        }

        private IEnumerable<string>? FindSpecializations(Paragraph paragraph)
        {
            string concatedRuns = string.Join(' ', paragraph.Elements<Run>()
                .SelectMany(r => r.Elements<Text>().Select(t => t.Text)));

            specializationsFinder.TryFind(concatedRuns, out var specializations);

            return specializations;
        }
    }
}
