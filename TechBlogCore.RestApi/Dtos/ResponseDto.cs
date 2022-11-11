namespace TechBlogCore.RestApi.Dtos
{
    public class ResponseDto<T>
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public T Data { get; set; }
    }
}
