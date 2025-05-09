namespace MovieApp.Domain.Results;

public class CallResult<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? ErrorMessage { get; }

    private CallResult(T? data)
    {
        IsSuccess = true;
        Data = data;
    }

    private CallResult(string errorMessage)
    {
        IsSuccess = false;
        ErrorMessage = errorMessage;
    }

    public static CallResult<T> Success(T? data) => new(data);
    public static CallResult<T> Failure(string errorMessage) => new(errorMessage);
}