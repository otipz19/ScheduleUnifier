using System.Text.Json.Serialization;

namespace ConsoleApp.Serialization.Models
{
    internal class GroupInfo
    {
        [JsonPropertyName("Назва")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("Час")]
        public string Time { get; set; } = default!;

        [JsonPropertyName("Тижні")]
        public string Weeks { get; set; } = default!;

        [JsonPropertyName("Аудиторія")]
        public string Classroom { get; set; } = default!;

        [JsonPropertyName("День тижня")]
        public string Day { get; set; } = default!;
    }
}
