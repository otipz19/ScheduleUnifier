using ConsoleApp.Interpreting.Models;

namespace ConsoleApp.Serialization.Models
{
    internal class Schedule : Dictionary<string, Faculty>
    {
        public void Add(RecordModel record)
        {
            if (!this.ContainsKey(record.Faculty))
            {
                this.Add(record.Faculty, new Faculty());
            }

            this[record.Faculty].Add(record);
        }
    }
}
