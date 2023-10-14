using ConsoleApp.Serialization;
using ScheduleUnifier.Interpreting.Models;
using ScheduleUnifier.Serialization.Models;

namespace ScheduleUnifierTests
{
    [TestFixture]
    internal class ScheduleSerializationTests
    {
        private const string DefaultDiscipline = "discipline";
        private const string DefaultFaculty = "faculty";
        private const string DefaultSpecialization = "specialization";

        private ScheduleHandler scheduleHandler;

        [SetUp]
        public void SetUp()
        {
            scheduleHandler = new ScheduleHandler();
        }

        [Test, Category("Positive")]
        public void Add_ThreeDistinctRecords_ContainsThreeGroupInfos()
        {
            IEnumerable<RecordModel> records = Enumerable.Range(1, 3).Select(i => CreateRecord(i.ToString()));

            foreach (var record in records)
            {
                scheduleHandler.Schedule.Add(record);
            }

            GroupInfo[] groups = scheduleHandler.Schedule
                .SelectMany(faculty => faculty.Value
                    .SelectMany(spec => spec.Value
                        .SelectMany(discipline => discipline.Value)))
                .ToArray();

            Assert.That(groups.Length, Is.EqualTo(3));
        }

        [Test, Category("Positive")]
        public void Add_ThreeSameRecords_ContainsOneGroupInfo()
        {
            IEnumerable<RecordModel> records = new[]
            {
                CreateRecord(1.ToString()),
                CreateRecord(1.ToString()),
                CreateRecord(1.ToString())
            };

            foreach (var record in records)
            {
                scheduleHandler.Schedule.Add(record);
            }

            GroupInfo[] groups = scheduleHandler.Schedule
                .SelectMany(faculty => faculty.Value
                    .SelectMany(spec => spec.Value
                        .SelectMany(discipline => discipline.Value)))
                .ToArray();

            Assert.That(groups.Length, Is.EqualTo(1));
        }

        private RecordModel CreateRecord(string valueForGroupInfo)
        {
            return new RecordModel()
            {
                Faculty = DefaultFaculty,
                Specialization = DefaultSpecialization,
                Discipline = DefaultDiscipline,
                Group = valueForGroupInfo,
                Classroom = valueForGroupInfo,
                Day = valueForGroupInfo,
                Time = valueForGroupInfo,
                Weeks = valueForGroupInfo
            };
        }
    }
}
