namespace TechBlogCore.RestApi.Helpers
{
    public class MessageException : Exception
    {
        public MessageException(string message) : base(message)
        {
        }
    }
}
