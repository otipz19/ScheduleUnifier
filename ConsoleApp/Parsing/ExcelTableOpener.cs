using ConsoleApp.Parsing.TableModels;
using OfficeOpenXml;

namespace ConsoleApp.Parsing
{
    internal class ExcelTableOpener : BaseTableOpener, ITableOpener
    {
        private ExcelPackage package = default!;


        protected override ITable OpenTable(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            package = new ExcelPackage(fileInfo);
            ExcelWorksheet excelWorksheet = package.Workbook.Worksheets[0];
            return new ExcelTable(excelWorksheet);
        }

        public void Close()
        {
            package.Dispose();
        }
    }
}
