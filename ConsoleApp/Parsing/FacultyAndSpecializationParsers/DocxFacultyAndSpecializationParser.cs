using ConsoleApp.Parsing.Exceptions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions;

namespace ConsoleApp.Parsing.FacultyAndSpecializationParsers
{
    internal class DocxFacultyAndSpecializationParser : IFacultyAndSpecializationParser
    {
        private readonly WordprocessingDocument document;

        public DocxFacultyAndSpecializationParser(WordprocessingDocument document)
        {
            this.document = document;
        }

        public (string faculty, IEnumerable<string> specializations) Parse()
        {
            string? faculty = null;
            IEnumerable<string> specializations = Enumerable.Empty<string>();

            var paragraphs = document.MainDocumentPart!.Document.Body!.Elements<Paragraph>();

            foreach (Paragraph paragraph in paragraphs)
            {
                if (!specializations.Any())
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
                foreach (Text text in run.Elements<Text>())
                {
                    if (faculty is null)
                    {
                        faculty = FindFaculty(text);
                    }
                }
            }

            if (faculty is null)
            {
                throw new NotFoundFacultyException();
            }

            if (!specializations.Any())
            {
                throw new NotFoundSpecializationsException();
            }

            return (faculty, specializations);
        }

        private string? FindFaculty(Text text)
        {
            if (text.Text.Contains("факультет", StringComparison.InvariantCultureIgnoreCase)
                && !text.Text.Contains("декан", StringComparison.InvariantCultureIgnoreCase))
            {
                return text.Text.Trim();
            }

            return null;
        }

        private IEnumerable<string> FindSpecializations(Paragraph paragraph)
        {
            string concatedRuns = string.Join(' ', paragraph.Elements<Run>()
                .SelectMany(r => r.Elements<Text>().Select(t => t.Text)));

            if (concatedRuns.Contains("спеціальність", StringComparison.InvariantCultureIgnoreCase))
            {
                //This regex pattern retrieves from string all substrings that contained inside either "" or «» quotes
                return Regex.Matches(concatedRuns, "(?<=\"|«)[^\"«]+(?=\"|»)")
                        .Select(m => m.Value.Trim())
                        .ToList();
            }

            return Enumerable.Empty<string>();
        }
    }
}
