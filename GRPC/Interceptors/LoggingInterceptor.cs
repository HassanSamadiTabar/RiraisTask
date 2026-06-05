using System.Diagnostics;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace RiraisTask.Grpc.Interceptors;

public class LoggingInterceptor(ILogger<LoggingInterceptor> logger) : Interceptor
{
    public const string CorrelationIdHeader = "x-correlation-id";

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var correlationId = context.RequestHeaders.GetValue(CorrelationIdHeader)
            ?? Guid.NewGuid().ToString("N");

        using (logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = correlationId,
            ["GrpcMethod"] = context.Method,
            ["Peer"] = context.Peer
        }))
        {
            var stopwatch = Stopwatch.StartNew();
            logger.LogInformation("gRPC call started");

            try
            {
                var response = await continuation(request, context);
                logger.LogInformation(
                    "gRPC call completed in {ElapsedMilliseconds}ms",
                    stopwatch.ElapsedMilliseconds);
                return response;
            }
            catch (RpcException ex)
            {
                logger.LogWarning(
                    ex,
                    "gRPC call failed with status {StatusCode} in {ElapsedMilliseconds}ms",
                    ex.StatusCode,
                    stopwatch.ElapsedMilliseconds);
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "gRPC call failed in {ElapsedMilliseconds}ms",
                    stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
