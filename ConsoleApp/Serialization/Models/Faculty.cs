using ConsoleApp.Interpreting.Models;

namespace ConsoleApp.Serialization.Models
{
    internal class Faculty : Dictionary<string, Specialization>
    {
        public void Add(RecordModel record)
        {
            if (!this.ContainsKey(record.Specialization))
            {
                this.Add(record.Specialization, new Specialization());
            }

            this[record.Specialization].Add(record);
        }
    }
}
