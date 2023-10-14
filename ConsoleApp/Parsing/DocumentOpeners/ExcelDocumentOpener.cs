using ScheduleUnifier.Parsing.FacultyAndSpecializationParsers;
using ScheduleUnifier.Parsing.TableModels;
using OfficeOpenXml;

namespace ScheduleUnifier.Parsing.TableOpeners
{
    internal class ExcelDocumentOpener : BaseDocumentOpener, IDocumentOpener
    {
        public ExcelDocumentOpener(string filePath) : base(filePath)
        {
        }

        public IEnumerable<ITable> Tables { get; private set; }

        public IFacultyAndSpecializationParser FacultyAndSpecializationParser { get; private set; }

        protected override void OpenDocument(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage(fileInfo);
            var excelWorksheets = package.Workbook.Worksheets;
            Tables = excelWorksheets.Select(ws => new ExcelTable(ws));
            FacultyAndSpecializationParser = new ExcelFacultyAndSpecializationParser(Tables);
        }
    }
}
