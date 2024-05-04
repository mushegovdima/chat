namespace Chat.Api.Models;

public class Message
{
    public required string UserName { get; set; }
    public required string Description { get; set; }
    public required long Timestamp { get; set; }
}
