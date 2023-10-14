﻿using ConsoleApp.Parsing.Exceptions;
using ConsoleApp.Parsing.FacultyAndSpecializationParsers;
using ConsoleApp.Parsing.TableModels;
using NPOI.HWPF;
using NPOI.HWPF.UserModel;

namespace ConsoleApp.Parsing.TableOpeners
{
    internal class DocTableOpener : BaseTableOpener, ITableOpener
    {
        public DocTableOpener(string filePath) : base(filePath)
        {
        }

        public ITable Table { get; private set; }

        public IFacultyAndSpecializationParser FacultyAndSpecializationParser { get; private set; }

        protected override void OpenDocument(string filePath)
        {
            HWPFDocument doc = new HWPFDocument(new FileStream(filePath, FileMode.Open));
            TableIterator tableIterator = new TableIterator(doc.GetRange());
            if (tableIterator.HasNext())
            {
                Table = new DocTable(tableIterator.Next());
            }
            else
            {
                throw new NotFoundTableException(filePath);
            }
        }
    }
}
