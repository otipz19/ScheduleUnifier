using ScheduleUnifier.Interpreting.Models;

namespace ScheduleUnifier.Serialization.Models
{
    public class Discipline : List<GroupInfo>
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
