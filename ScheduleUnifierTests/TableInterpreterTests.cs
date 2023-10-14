using ScheduleUnifier.Interpreting;
using ScheduleUnifier.Interpreting.Models;
using ScheduleUnifier.Parsing.Models;

namespace ScheduleUnifierTests
{
    [TestFixture]
    internal class TableInterpreterTests
    {
        private const string DefaultDay = "day";
        private const string DefaultTime = "time";
        private const string DefaultClass = "class";
        private const string DefaultGroup = "1";
        private const string DefaultWeeks = "weeks";
        private const string DefaultDiscipline = "discipline";
        private const string DefaultFaculty = "faculty";

        private TableInterpreter interpreter;

        [SetUp]
        public void SetUp()
        {
            this.interpreter = new TableInterpreter();
        }

        [Test, Category("Negative")]
        public void Interpret_NoSpecializations_ThrowsException()
        {
            var data = new ParsedTable()
            {
                Specializations = Enumerable.Empty<string>(),
            };

            Assert.Throws<Exception>(() => interpreter.Interpret(data));
        }

        [Test, Category("Negative")]
        public void Interpret_IncorrectGroup_ThrowsException()
        {
            const string Specialization = "spec1";

            var data = new ParsedTable()
            {
                Faculty = DefaultFaculty,
                Specializations = new string[] { Specialization },
                Rows = new ParsedRow[]
                {
                    CreateDefaultParsedRow(Specialization) with {Group = "incorrect group"},
                },
            };

            Assert.Throws<Exception>(() => interpreter.Interpret(data));
        }

        [Test, Category("Positive")]
        public void Interpret_DistantClassroom_ProperlyClassroom()
        {
            const string Specialization = "spec1";

            var data = new ParsedTable()
            {
                Faculty = DefaultFaculty,
                Specializations = new string[] { Specialization },
                Rows = new ParsedRow[]
                {
                    CreateDefaultParsedRow(Specialization) with {Classroom = "д"},
                },
            };
            
            var records = interpreter.Interpret(data);

            Assert.That(records.Count(), Is.EqualTo(1));
            Assert.That(records.First().Classroom, Is.EqualTo("Дистанційно"));
        }

        [Test, Category("Positive")]
        public void Interpret_OneSpecialization_CreatesOneRecord()
        {
            const string Specialization = "spec1";

            var data = new ParsedTable()
            {
                Faculty = DefaultFaculty,
                Specializations = new string[] { Specialization },
                Rows = new ParsedRow[]
                {
                    CreateDefaultParsedRow(),
                },
            };

            IEnumerable<RecordModel> records = interpreter.Interpret(data);

            Assert.That(records.Count(), Is.EqualTo(1));
            Assert.That(records.First().Specialization, Is.EqualTo(Specialization));
            Assert.That(records.First().Discipline, Is.EqualTo(DefaultDiscipline));
        }

        [Test, Category("Positive")]
        public void Interpret_ThreeSpecializations_RowsWithSingleSpecialization_CreatesOneRecordForEachRow()
        {
            string[] specializations = Enumerable.Range(1, 3).Select(i => $"spec{i}").ToArray();

            var data = new ParsedTable()
            {
                Faculty = DefaultFaculty,
                Specializations = specializations,
                Rows = new ParsedRow[]
                {
                    CreateDefaultParsedRow(specializations[0]),
                    CreateDefaultParsedRow(specializations[1]),
                    CreateDefaultParsedRow(specializations[2]),
                },
            };

            IEnumerable<RecordModel> records = interpreter.Interpret(data);

            Assert.That(records.Count(), Is.EqualTo(3));
            foreach (var record in records)
            {
                Assert.That(record.Specialization, Is.AnyOf(specializations));
                Assert.That(record.Discipline, Is.EqualTo(DefaultDiscipline));
            }
        }

        [Test, Category("Positive")]
        public void Interpret_ThreeSpecializations_RowsWithMultipleSpecializations_CreatesMultimpleRecordsForEachRow()
        {
            string[] specializations = Enumerable.Range(1, 3).Select(i => $"spec{i}").ToArray();

            var data = new ParsedTable()
            {
                Faculty = DefaultFaculty,
                Specializations = specializations,
                Rows = new ParsedRow[]
                {
                    CreateDefaultParsedRow($"{specializations[0]}+{specializations[1]}"),
                    CreateDefaultParsedRow($"{specializations[1]}+{specializations[2]}"),
                    CreateDefaultParsedRow($"{specializations[0]}+{specializations[1]}+{specializations[2]}"),
                },
            };

            IEnumerable<RecordModel> records = interpreter.Interpret(data);

            Assert.That(records.Count(), Is.EqualTo(7));
            foreach (var record in records)
            {
                Assert.That(record.Specialization, Is.AnyOf(specializations));
                Assert.That(record.Discipline, Is.EqualTo(DefaultDiscipline));
            }
        }

        private ParsedRow CreateDefaultParsedRow(string? specialization = null)
        {
            string discipline = DefaultDiscipline;
            if(specialization is not null)
            {
                discipline += $" ({specialization})";
            }

            return new ParsedRow()
            {
                Day = DefaultDay,
                Time = DefaultTime,
                Discipline = discipline,
                Classroom = DefaultClass,
                Group = DefaultGroup,
                Weeks = DefaultWeeks,
            };
        }
    }
}
