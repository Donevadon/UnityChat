using System.Text.Json.Serialization;

namespace Server;

public interface IData
{
    [JsonPropertyName("nick")]
    string Nick { get; set; }
    
    [JsonPropertyName("color")]
    string Color { get; set; }
    
    [JsonPropertyName("status")]
    Status Status { get; set; }
}