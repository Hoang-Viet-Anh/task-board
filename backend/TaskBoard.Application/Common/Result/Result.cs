namespace TaskBoard.Application.Common.Result;

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T Value { get; }
    public Exception Error { get; }

    protected Result(bool isSuccess, T value, Exception error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) =>
        new Result<T>(true, value, null);

    public static Result<T> Failure(Exception error) =>
        new Result<T>(false, default, error);
}
