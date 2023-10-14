using ScheduleUnifier.Parsing.Exceptions;
using ScheduleUnifier.Parsing.FacultyAndSpecializationParsers;
using ScheduleUnifier.Parsing.TableModels;
using NPOI.HWPF;
using NPOI.HWPF.UserModel;

namespace ScheduleUnifier.Parsing.TableOpeners
{
    internal class DocDocumentOpener : BaseDocumentOpener, IDocumentOpener
    {
        private List<ITable> tables = new List<ITable>();

        public DocDocumentOpener(string filePath) : base(filePath)
        {
        }

        public IEnumerable<ITable> Tables => tables;

        public IFacultyAndSpecializationParser FacultyAndSpecializationParser { get; private set; }

        protected override void OpenDocument(string filePath)
        {
            HWPFDocument doc = new HWPFDocument(new FileStream(filePath, FileMode.Open));
            TableIterator tableIterator = new TableIterator(doc.GetRange());
            while(tableIterator.HasNext())
            {
                tables.Add(new DocTable(tableIterator.Next()));
            }

            if(!tables.Any())
            {
                throw new NotFoundTableException(filePath);
            }
        }
    }
}
