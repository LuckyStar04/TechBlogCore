namespace TechBlogCore.RestApi.Dtos
{
    public class ChatCompleteInputDto
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }

    public class ChatDto
    {
        public bool isMe { get; set; }
        public string content { get; set; }
        public DateTime time { get; set; }
    }

    public class ChatCompleteDto
    {
        public string model { get; set; }
        public IEnumerable<ChatCompleteMessageDto> messages { get; set; }
    }

    public class ChatCompleteMessageDto
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public class ChatResponseDto
    {
        public List<ChatMessageDto> choices { get; set; }
    }

    public class ChatMessageDto
    {
        public ChatCompleteMessageDto message { get; set; }
        public string finish_reason { get; set; }
        public int index { get; set; }
    }
}
