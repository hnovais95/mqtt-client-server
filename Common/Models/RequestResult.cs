using System.Text.Json;

namespace Common.Models
{
    public enum RequestResultCode
    {
        Success,
        Failure
    }

    public class RequestResult
    {
        public RequestResultCode ResultCode { get; set; }
        public string Message { get; set; }
        public object Body { get; set; }

        public T DeserializeBody<T>()
        {
            var json = ((JsonElement)Body).GetRawText();
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
