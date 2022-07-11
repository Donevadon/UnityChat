using System.Text.Json.Serialization;

namespace Server.ConnectionClient;

public class UserData : IData
{
    [JsonPropertyName("nick")] public string Nick { get; set; }

    [JsonPropertyName("color")] public string Color { get; set; }

    [JsonPropertyName("status")] public Status Status { get; set; }
}