namespace ShopHub.WebApi.Common;

public class ServiceResult<T> 
{
    private ServiceResult(bool success, T? data = default, string? errorMessage = null)
    {
        Success = success;
        Data = data;
        ErrorMessage = errorMessage;
    }

    public bool Success { get; private set; }
    public T? Data { get; private set; }
    public string? ErrorMessage { get; private set;}
    public static ServiceResult<T> Ok(T data)=> new ServiceResult<T>(true, data);
    public static ServiceResult<T> Fail(string errorMessage) => new ServiceResult<T>(false, default, errorMessage);
}

public class ServiceResult
{
    public bool Success { get; private set; }
    public string? ErrorMessage { get; private set; }
    private ServiceResult(bool success, string? errorMessage = null)
    {
        Success = success;
        ErrorMessage = errorMessage;
    }
    public static ServiceResult Ok() => new ServiceResult(true);
    public static ServiceResult Fail(string errorMessage) => new ServiceResult(false, errorMessage);
}
