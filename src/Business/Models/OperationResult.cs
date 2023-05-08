namespace Business.Models;

public class OperationResult<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }

    public static OperationResult<T> SuccessResult(T data)
    {
        return new OperationResult<T> { Success = true, Data = data };
    }

    public static OperationResult<T> FailureResult(string message)
    {
        return new OperationResult<T> { Success = false, Message = message };
    }
}
