namespace GlobalCoders.PSP.BackendApi.Base.Extensions;

public static class LoggerExtension
{
    public static void LogExceptionError(this ILogger logger, Exception exception, string methodName)
    {
        logger.LogError(exception, "Exception occurred in {MethodName}", methodName);
    }

    public static void LogExceptionError(
        this ILogger logger,
        Exception exception,
        string serviceName,
        string methodName)
    {
        logger.LogError(exception, "Exception occurred in {ServiceName} {MethodName}", serviceName, methodName);
    }
}
