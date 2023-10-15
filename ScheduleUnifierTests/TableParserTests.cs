using ScheduleUnifier.Exceptions;
using ScheduleUnifier.Parsing.Models;
using ScheduleUnifier.Parsing.TableModels;
using ScheduleUnifier.Parsing.TableParsers;

namespace ScheduleUnifierTests
{
    [TestFixture]
    internal class TableParserTests
    {
        [Test, Category("Positive")]
        public void Parse_NotFirstHeader_ReturnsValidParsedTable()
        {
            var tableMock = new TableMock(new string[,]
            {
                { "", "", "", "", "", "" },
                { "", "", "", "", "", "" },
                { "", "", "", "", "Тижні", "" },
                { "Day-1", "Time-1", "1", "1", "1", "1" },
                { "Day-2", "Time-2", "2", "2", "2", "2" },
                { "Day-3", "Time-3", "3", "3", "3", "3" },
                { "Day-4", "Time-4", "4", "4", "4", "4" },
                { "Day-5", "Time-5", "5", "5", "5", "5" },
                { "Day-6", "Time-6", "6", "6", "6", "6" },
                { "Day-7", "Time-7", "7", "7", "7", "7" },
                { "Day-8", "Time-8", "8", "8", "8", "8" }
            });

            ParsedRow[] expectedRows = new[]
            {
                CreateParsedRow("Day-1", "Time-1", 1),
                CreateParsedRow("Day-2", "Time-2", 2),
                CreateParsedRow("Day-3", "Time-3", 3),
                CreateParsedRow("Day-4", "Time-4", 4),
                CreateParsedRow("Day-5", "Time-5", 5),
                CreateParsedRow("Day-6", "Time-6", 6),
                CreateParsedRow("Day-7", "Time-7", 7),
                CreateParsedRow("Day-8", "Time-8", 8),
            };

            RunPositiveTest(tableMock, expectedRows);
        }

        [Test, Category("Positive")]
        public void Parse_AllDistinctRows_ReturnsValidParsedTable()
        {
            var tableMock = new TableMock(new string[,]
            {
                { "День", "", "", "", "", "" },
                { "Day-1", "Time-1", "1", "1", "1", "1" },
                { "Day-2", "Time-2", "2", "2", "2", "2" },
                { "Day-3", "Time-3", "3", "3", "3", "3" },
                { "Day-4", "Time-4", "4", "4", "4", "4" },
                { "Day-5", "Time-5", "5", "5", "5", "5" },
                { "Day-6", "Time-6", "6", "6", "6", "6" },
                { "Day-7", "Time-7", "7", "7", "7", "7" },
                { "Day-8", "Time-8", "8", "8", "8", "8" }
            });

            ParsedRow[] expectedRows = new[]
            {
                CreateParsedRow("Day-1", "Time-1", 1),
                CreateParsedRow("Day-2", "Time-2", 2),
                CreateParsedRow("Day-3", "Time-3", 3),
                CreateParsedRow("Day-4", "Time-4", 4),
                CreateParsedRow("Day-5", "Time-5", 5),
                CreateParsedRow("Day-6", "Time-6", 6),
                CreateParsedRow("Day-7", "Time-7", 7),
                CreateParsedRow("Day-8", "Time-8", 8),
            };

            RunPositiveTest(tableMock, expectedRows);
        }

        [Test, Category("Positive")]
        public void Parse_ValidTable_ReturnsValidParsedTable()
        {
            var tableMock = new TableMock(new string[,]
            {
                { "День", "", "", "", "", "" },
                { "Day-1", "Time-1", "1", "1", "1", "1" },
                { "", "", "2", "2", "2", "2" },
                { "", "Time-2", "3", "3", "3", "3" },
                { "", "", "4", "4", "4", "4" },
                { "Day-2", "Time-3", "5", "5", "5", "5" },
                { "", "", "6", "6", "6", "6" },
                { "", "Time-4", "7", "7", "7", "7" },
                { "", "", "8", "8", "8", "8" }
            });

            ParsedRow[] expectedRows = new[]
            {
                CreateParsedRow("Day-1", "Time-1", 1),
                CreateParsedRow("Day-1", "Time-1", 2),
                CreateParsedRow("Day-1", "Time-2", 3),
                CreateParsedRow("Day-1", "Time-2", 4),
                CreateParsedRow("Day-2", "Time-3", 5),
                CreateParsedRow("Day-2", "Time-3", 6),
                CreateParsedRow("Day-2", "Time-4", 7),
                CreateParsedRow("Day-2", "Time-4", 8),
            };

            RunPositiveTest(tableMock, expectedRows);
        }

        [Test, Category("Positive")]
        public void Parse_ValidTable_ReturnsOnlyFullRows()
        {
            //Arrange
            var tableMock = new TableMock(new string[,]
            {
                { "День", "", "", "", "", "" },
                { "Day-1", "Time-1", "1", "1", "1", "1" },
                { "", "", "", "", "", "" },
                { "", "Time-2", "", "", "", "" },
                { "", "", "4", "4", "4", "4" },
                { "Day-2", "Time-3", "", "", "", "" },
                { "", "", "6", "6", "6", "6" },
                { "", "Time-4", "7", "7", "7", "7" },
                { "", "", "", "", "", "" }
            });

            ParsedRow[] expectedRows = new[]
            {
                CreateParsedRow("Day-1", "Time-1", 1),
                CreateParsedRow("Day-1", "Time-2", 4),
                CreateParsedRow("Day-2", "Time-3", 6),
                CreateParsedRow("Day-2", "Time-4", 7),
            };

            RunPositiveTest(tableMock, expectedRows);
        }

        [Test, Category("Negative")]
        public void IsTargetTable_MissingSavedDay_ThrowsException()
        {
            var tableMock = new TableMock(new string[,]
            {
                { "День", "", "", "", "", "" },
                { "", "Time-1", "1", "1", "1", "1" },
                { "Day-2", "Time-2", "2", "2", "2", "2" },
                { "Day-3", "Time-3", "3", "3", "3", "3" },
                { "Day-4", "Time-4", "4", "4", "4", "4" },
                { "Day-5", "Time-5", "5", "5", "5", "5" },
                { "Day-6", "Time-6", "6", "6", "6", "6" },
                { "Day-7", "Time-7", "7", "7", "7", "7" },
                { "Day-8", "Time-8", "8", "8", "8", "8" }
            });
            var parser = new TableParser(tableMock);

            Assert.Throws<MissingSavedDayException>(() => parser.ParseRows());
        }

        [Test, Category("Negative")]
        public void Parse_MissingSavedTime_ThrowsException()
        {
            var tableMock = new TableMock(new string[,]
            {
                { "День", "", "", "", "", "" },
                { "Day-1", "", "1", "1", "1", "1" },
                { "Day-2", "Time-2", "2", "2", "2", "2" },
                { "Day-3", "Time-3", "3", "3", "3", "3" },
                { "Day-4", "Time-4", "4", "4", "4", "4" },
                { "Day-5", "Time-5", "5", "5", "5", "5" },
                { "Day-6", "Time-6", "6", "6", "6", "6" },
                { "Day-7", "Time-7", "7", "7", "7", "7" },
                { "Day-8", "Time-8", "8", "8", "8", "8" }
            });
            var parser = new TableParser(tableMock);

            Assert.Throws<MissingSavedTimeException>(() => parser.ParseRows());
        }

        [Test, Category("Negative")]
        public void Parse_InvalidTableWithoutHeader_ReturnsFalse()
        {
            var tableMock = new TableMock(new string[,]
            {
                { "1", "1", "1", "1", "1", "1" },
                { "2", "2", "2", "2", "2", "2" },
            });
            var parser = new TableParser(tableMock);


            Assert.IsFalse(parser.IsTargetTable());
        }

        private void RunPositiveTest(ITable tableMock, ParsedRow[] expectedRows)
        {
            //Arrange
            var parser = new TableParser(tableMock);

            //Act
            IEnumerable<ParsedRow> parsedRows = parser.ParseRows();

            //Assert
            var actualRows = parsedRows.ToArray();
            Assert.That(actualRows, Has.Length.EqualTo(expectedRows.Length));
            for (int i = 0; i < actualRows.Length; i++)
            {
                Assert.That(actualRows[i], Is.EqualTo(expectedRows[i]));
            }
        }

        private ParsedRow CreateParsedRow(string day, string time, int otherValues)
        {
            string value = otherValues.ToString();
            return new ParsedRow()
            {
                Day = day,
                Time = time,
                Classroom = value,
                Discipline = value,
                Group = value,
                Weeks = value,
            };
        }
    }
}
