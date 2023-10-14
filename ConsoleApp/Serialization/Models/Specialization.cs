using ScheduleUnifier.Interpreting.Models;

namespace ScheduleUnifier.Serialization.Models
{
    public class Specialization : Dictionary<string, Discipline>
    {
        public void Add(RecordModel record)
        {
            if (!this.ContainsKey(record.Discipline))
            {
                this.Add(record.Discipline, new Discipline());
            }

            this[record.Discipline].Add(record);
        }
    }
}
