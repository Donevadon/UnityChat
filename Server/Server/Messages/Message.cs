using System.Text.Json.Serialization;

namespace Server.Messages;

public class Message : IMessage
{
    [JsonPropertyName("nick")] 
    public string Nick { get; set; }

    [JsonPropertyName("color")] 
    public string Color { get; set; }

    [JsonPropertyName("text")] 
    public string Text { get; set; }

    public DateTime Timestamp { get; set; }
}