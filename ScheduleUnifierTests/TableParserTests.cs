using ConsoleApp.Parsing.Exceptions;
using ScheduleUnifier.Parsing.Exceptions;
using ScheduleUnifier.Parsing.Models;
using ScheduleUnifier.Parsing.TableParsers;
using ScheduleUnifierTests.Mocks;

namespace ScheduleUnifierTests
{
    [TestFixture]
    internal class TableParserTests
    {
        [Test, Category("Positive")]
        public void Parse_AllDistinctRows_ReturnsValidParsedTable()
        {
            //Arrange
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
            var facultyParserMock = new FacultyAndSpecializationParserMock();
            var openerMock = new TableOpenerMock(tableMock, facultyParserMock);
            var parser = new TableParser(openerMock);

            string expectedFaculty = facultyParserMock.Parse().faculty;
            string[] expectedSpecializations = facultyParserMock.Parse().specializations.ToArray();
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

            //Act
            ParsedTable parsedTable = parser.Parse();

            //Assert
            Assert.That(parsedTable.Faculty, Is.EqualTo(expectedFaculty));

            var actualSpecializations = parsedTable.Specializations.ToArray();
            Assert.That(actualSpecializations.Length, Is.EqualTo(expectedSpecializations.Length));
            for (int i = 0; i < actualSpecializations.Length; i++)
            {
                Assert.That(actualSpecializations[i], Is.EqualTo(expectedSpecializations[i]));
            }

            var actualRows = parsedTable.Rows.ToArray();
            Assert.That(actualRows.Length, Is.EqualTo(expectedRows.Length));
            for (int i = 0; i < actualRows.Length; i++)
            {
                Assert.That(actualRows[i], Is.EqualTo(expectedRows[i]));
            }
        }

        [Test, Category("Positive")]
        public void Parse_ValidTable_ReturnsValidParsedTable()
        {
            //Arrange
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
            var facultyParserMock = new FacultyAndSpecializationParserMock();
            var openerMock = new TableOpenerMock(tableMock, facultyParserMock);
            var parser = new TableParser(openerMock);

            string expectedFaculty = facultyParserMock.Parse().faculty;
            string[] expectedSpecializations = facultyParserMock.Parse().specializations.ToArray();
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

            //Act
            ParsedTable parsedTable = parser.Parse();

            //Assert
            Assert.That(parsedTable.Faculty, Is.EqualTo(expectedFaculty));

            var actualSpecializations = parsedTable.Specializations.ToArray();
            Assert.That(actualSpecializations.Length, Is.EqualTo(expectedSpecializations.Length));
            for (int i = 0; i < actualSpecializations.Length; i++)
            {
                Assert.That(actualSpecializations[i], Is.EqualTo(expectedSpecializations[i]));
            }

            var actualRows = parsedTable.Rows.ToArray();
            Assert.That(actualRows.Length, Is.EqualTo(expectedRows.Length));
            for (int i = 0; i < actualRows.Length; i++)
            {
                Assert.That(actualRows[i], Is.EqualTo(expectedRows[i]));
            }
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
            var facultyParserMock = new FacultyAndSpecializationParserMock();
            var openerMock = new TableOpenerMock(tableMock, facultyParserMock);
            var parser = new TableParser(openerMock);

            string expectedFaculty = facultyParserMock.Parse().faculty;
            string[] expectedSpecializations = facultyParserMock.Parse().specializations.ToArray();
            ParsedRow[] expectedRows = new[]
            {
                CreateParsedRow("Day-1", "Time-1", 1),
                CreateParsedRow("Day-1", "Time-2", 4),
                CreateParsedRow("Day-2", "Time-3", 6),
                CreateParsedRow("Day-2", "Time-4", 7),
            };

            //Act
            ParsedTable parsedTable = parser.Parse();

            //Assert
            Assert.That(parsedTable.Faculty, Is.EqualTo(expectedFaculty));

            var actualSpecializations = parsedTable.Specializations.ToArray();
            Assert.That(actualSpecializations.Length, Is.EqualTo(expectedSpecializations.Length));
            for (int i = 0; i < actualSpecializations.Length; i++)
            {
                Assert.That(actualSpecializations[i], Is.EqualTo(expectedSpecializations[i]));
            }

            var actualRows = parsedTable.Rows.ToArray();
            Assert.That(actualRows.Length, Is.EqualTo(expectedRows.Length));
            for (int i = 0; i < actualRows.Length; i++)
            {
                Assert.That(actualRows[i], Is.EqualTo(expectedRows[i]));
            }
        }

        [Test, Category("Negative")]
        public void Parse_MissingSavedDay_ThrowsException()
        {
            //Arrange
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
            var facultyParserMock = new FacultyAndSpecializationParserMock();
            var openerMock = new TableOpenerMock(tableMock, facultyParserMock);
            var parser = new TableParser(openerMock);

            //Assert
            Assert.Throws<MissingSavedDayException>(() => parser.Parse());
        }

        [Test, Category("Negative")]
        public void Parse_MissingSavedTime_ThrowsException()
        {
            //Arrange
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
            var facultyParserMock = new FacultyAndSpecializationParserMock();
            var openerMock = new TableOpenerMock(tableMock, facultyParserMock);
            var parser = new TableParser(openerMock);

            //Assert
            Assert.Throws<MissingSavedTimeException>(() => parser.Parse());
        }

        [Test, Category("Negative")]
        public void Parse_InvalidTableWithoutHeader_ThrowsException()
        {
            //Arrange
            var tableMock = new TableMock(new string[,]
            {
                { "1", "1", "1", "1", "1", "1" },
                { "2", "2", "2", "2", "2", "2" },
            });
            var facultyParserMock = new FacultyAndSpecializationParserMock();
            var openerMock = new TableOpenerMock(tableMock, facultyParserMock);
            var parser = new TableParser(openerMock);

            //Assert
            Assert.Throws<NotFoundHeaderException>(() => parser.Parse());
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
