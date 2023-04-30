namespace TechBlogCore.RestApi.Dtos
{
    public class ChatCompleteInputDto
    {
        public string Role { get; set; }
        public string Content { get; set; }
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
        public string id { get; set; }
        public string @object { get; set; }
        public int created { get; set; }
        public string model { get; set; }
        public int MyProperty { get; set; }
    }
}
