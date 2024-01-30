namespace SignalR_Chat.Dtos
{
    public class MessageDto
    {
        public string Content { get; set; }
        public string? From { get; set; } = null;
        public string? To { get; set; } = null;

        public MessageDto(string content, string? from = null, string? to = null)
        {
            Content = content;
            From = from;
            To = to;
        }

    }
}
