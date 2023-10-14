using b2xtranslator.DocFileFormat;
using b2xtranslator.OpenXmlLib.WordprocessingML;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.WordprocessingMLMapping;
using static b2xtranslator.OpenXmlLib.OpenXmlPackage;

namespace ConsoleApp.Parsing.Converters
{
    internal class B2xConverter : IDocToDocxConverter
    {
        public string Convert(string docPath)
        {
            FileStream streamDocFile = new FileStream(docPath, FileMode.Open);
            StructuredStorageReader reader = new StructuredStorageReader(streamDocFile);
            WordDocument doc = new WordDocument(reader);
            string docxPath = docPath + "x";
            WordprocessingDocument docx = WordprocessingDocument.Create(docxPath, DocumentType.Document);
            Converter.Convert(doc, docx);
            return docxPath;
        }
    }
}
