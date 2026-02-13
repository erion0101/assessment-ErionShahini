namespace Services.DTOs;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public string? Error { get; private set; }
    public T? Data { get; private set; }

    private Result(bool isSuccess, string? error, T? data)
    {
        IsSuccess = isSuccess;
        Error = error;
        Data = data;
    }

    public static Result<T> Success(T data) => new(true, null, data);
    public static Result<T> Failure(string error) => new(false, error, default);
}
