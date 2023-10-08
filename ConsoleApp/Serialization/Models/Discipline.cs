using ConsoleApp.Interpreting.Models;

namespace ConsoleApp.Serialization.Models
{
    internal class Discipline : List<GroupInfo>
    {
        public void Add(RecordModel record)
        {
            var group = new GroupInfo()
            {
                Classroom = record.Classroom,
                Day = record.Day,
                Name = record.Group,
                Time = record.Time,
                Weeks = record.Weeks,
            };

            this.Add(group);
        }
    }
}
