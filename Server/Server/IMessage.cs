using System.Text.Json.Serialization;

namespace Server;

public interface IMessage
{
    [JsonPropertyName("nick")]
    string Nick { get; set; }
    
    [JsonPropertyName("color")]
    string Color { get; set; }
    
    [JsonPropertyName("text")]
    string Text { get; set; }
}