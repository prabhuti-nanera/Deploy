namespace CRC.WebPortal.BlazorWebApp.Models;

public class BaseResponse<T>
{
    public bool Succeeded { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public IEnumerable<string>? Errors { get; set; }

    public static BaseResponse<T> Success(T data, string message = "")
    {
        return new BaseResponse<T>
        {
            Succeeded = true,
            Data = data,
            Message = message
        };
    }

    public static BaseResponse<T> Failure(string message, IEnumerable<string>? errors = null)
    {
        return new BaseResponse<T>
        {
            Succeeded = false,
            Message = message,
            Errors = errors
        };
    }
}

public class BaseResponse : BaseResponse<object>
{
    public static BaseResponse Success(string message = "") => new() { Succeeded = true, Message = message };
    public new static BaseResponse Failure(string message, IEnumerable<string>? errors = null) => 
        new() { Succeeded = false, Message = message, Errors = errors };
}
