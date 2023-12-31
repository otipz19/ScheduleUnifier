﻿using DocumentFormat.OpenXml.Wordprocessing;
using ScheduleUnifier.Exceptions;

namespace ScheduleUnifier.Parsing.TableModels
{
    internal class DocxTable : ITable
    {
        private readonly Table table;

        private string[,] cells;

        public DocxTable(Table table)
        {
            this.table = table;

            var rows = table.Elements<TableRow>();
            int rowCount = rows.Count();
            var cols = rows.First().Elements<TableCell>();
            int colCount = cols.Count();

            cells = new string[rowCount, colCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    cells[i, j] = GetStringFromWordTable(i, j);
                }
            }
        }

        public string this[int row, int col]
        {
            get
            {
                try
                {
                    return cells[row, col];
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public string GetStringFromWordTable(int row, int col)
        {
            try
            {
                TableCell cell = table.Elements<TableRow>().ElementAt(row).Elements<TableCell>().ElementAt(col);
                return cell.InnerText;
            }
            catch
            {
                return string.Empty;
            }
        }

        public int GetLastNotEmptyRow()
        {
            TableRow[] rows = table.Elements<TableRow>().ToArray();
            if(rows.Length == 0)
            {
                throw new EmptyTableException();
            }
            int lastNotEmptyRowIndex = rows.Length - 1;
            while (!IsRowNotEmpty(rows[lastNotEmptyRowIndex]))
            {
                lastNotEmptyRowIndex--;
                if(lastNotEmptyRowIndex < 0)
                {
                    throw new EmptyTableException();
                }
            }
            return lastNotEmptyRowIndex;
        }

        private bool IsRowNotEmpty(TableRow row)
        {
            return row.Elements<TableCell>()
                .Any(c => !string.IsNullOrEmpty(c.InnerText));
        }
    }
}
