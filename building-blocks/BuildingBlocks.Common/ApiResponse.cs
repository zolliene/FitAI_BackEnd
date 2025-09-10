using System;


public class ApiResponse
{
    public bool Success { get; init; }
    public string? Message { get; init; }

    public static ApiResponse Fail(string message)
        => new() { Success = false, Message = message };
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; init; }

    public static ApiResponse<T> Ok(T data, string? message = null)
        => new() { Success = true, Data = data, Message = message };
}

