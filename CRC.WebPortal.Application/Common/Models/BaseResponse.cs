namespace CRC.WebPortal.Application.Common.Models;

public class BaseResponse
{
    public bool Succeeded { get; set; }
    public string[]? Errors { get; set; }
    public string? Message { get; set; }

    public static BaseResponse Success(string? message = null) => new() { Succeeded = true, Message = message };
    public static BaseResponse Failure(string[] errors) => new() { Succeeded = false, Errors = errors };
}

public class BaseResponse<T> : BaseResponse
{
    public T? Data { get; set; }

    public static BaseResponse<T> Success(T data, string? message = null) => new() 
    { 
        Succeeded = true, 
        Data = data, 
        Message = message 
    };
    
    public new static BaseResponse<T> Failure(string[] errors) => new() 
    { 
        Succeeded = false, 
        Errors = errors 
    };
}
