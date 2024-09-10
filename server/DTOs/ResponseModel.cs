
namespace server.DTOs;

public enum ResponseStatus
{
    Success,
    Error
}

public class ResponseModel<T>
{
    public int Code { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}