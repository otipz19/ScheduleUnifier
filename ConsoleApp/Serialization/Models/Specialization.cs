using ConsoleApp.Interpreting.Models;

namespace ConsoleApp.Serialization.Models
{
    internal class Specialization : Dictionary<string, Discipline>
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
