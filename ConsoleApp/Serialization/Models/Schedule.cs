using ScheduleUnifier.Interpreting.Models;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace ScheduleUnifier.Serialization.Models
{
    public class Schedule : Dictionary<string, Faculty>
    {
        public void Add(RecordModel record)
        {
            if (!this.ContainsKey(record.Faculty))
            {
                this.Add(record.Faculty, new Faculty());
            }

            this[record.Faculty].Add(record);
        }

        public string ToJsonString()
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            return JsonSerializer.Serialize<Schedule>(this, options);
        }
    }
}
