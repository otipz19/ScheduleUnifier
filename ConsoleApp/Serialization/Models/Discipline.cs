using ScheduleUnifier.Interpreting.Models;

namespace ScheduleUnifier.Serialization.Models
{
    public class Discipline : HashSet<GroupInfo>
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

            if (!this.Contains(group))
            {
                this.Add(group);
            }
        }
    }
}
