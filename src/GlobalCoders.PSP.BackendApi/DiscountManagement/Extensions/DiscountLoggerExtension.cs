using System;
using Microsoft.Extensions.Logging;

namespace GlobalCoders.PSP.BackendApi.DiscountManagment.Extensions
{
    public static class DiscountLoggerExtension
    {
        /// <summary>
        /// Logs an exception with additional context, such as the method name where the exception occurred.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="methodName">The method where the exception occurred.</param>
        public static void LogExceptionError(this ILogger logger, Exception exception, string methodName)
        {
            logger.LogError(exception, "An exception occurred in method {MethodName}: {Message}", methodName, exception.Message);
        }

        /// <summary>
        /// Logs a generic operation performed by a specific user.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="operation">The name of the operation being performed.</param>
        /// <param name="userId">The ID of the user performing the operation.</param>
        public static void LogOperation(this ILogger logger, string operation, Guid userId)
        {
            logger.LogInformation("Operation '{Operation}' performed by User ({UserId})", operation, userId);
        }

        /// <summary>
        /// Logs an entity-specific operation, such as creation or update, with additional context.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="operation">The operation performed (e.g., Created, Updated).</param>
        /// <param name="entityName">The name of the entity being operated on.</param>
        /// <param name="entityId">The ID of the entity being operated on.</param>
        public static void LogEntityOperation(this ILogger logger, string operation, string entityName, Guid entityId)
        {
            logger.LogInformation("Entity '{EntityName}' with ID ({EntityId}) was {Operation}", entityName, entityId, operation);
        }
    }
}
