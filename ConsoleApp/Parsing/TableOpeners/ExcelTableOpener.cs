using ConsoleApp.Parsing.FacultyAndSpecializationParsers;
using ConsoleApp.Parsing.TableModels;
using OfficeOpenXml;

namespace ConsoleApp.Parsing.TableOpeners
{
    internal class ExcelTableOpener : BaseTableOpener, ITableOpener
    {
        private ExcelPackage package = default!;

        public ExcelTableOpener(string filePath) : base(filePath)
        {
        }

        public ITable Table { get; private set; }

        public IFacultyAndSpecializationParser FacultyAndSpecializationParser { get; private set; }

        protected override void OpenDocument(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            package = new ExcelPackage(fileInfo);
            ExcelWorksheet excelWorksheet = package.Workbook.Worksheets[0];
            Table = new ExcelTable(excelWorksheet);
            FacultyAndSpecializationParser = new ExcelFacultyAndSpecializationParser(Table);
        }

        public void Close()
        {
            package.Dispose();
        }
    }
}
