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
            logger.LogInformation("gRPC call started for {GrpcMethod}", context.Method);

            try
            {
                var response = await continuation(request, context);
                logger.LogInformation(
                    "gRPC call completed for {GrpcMethod} in {ElapsedMilliseconds}ms",
                    context.Method,
                    stopwatch.ElapsedMilliseconds);
                return response;
            }
            catch (RpcException ex) when (IsExpectedClientError(ex.StatusCode))
            {
                logger.LogInformation(
                    "gRPC call rejected for {GrpcMethod} with {StatusCode}: {Detail} in {ElapsedMilliseconds}ms",
                    context.Method,
                    ex.StatusCode,
                    ex.Status.Detail,
                    stopwatch.ElapsedMilliseconds);
                throw;
            }
            catch (RpcException ex)
            {
                logger.LogWarning(
                    "gRPC call failed for {GrpcMethod} with {StatusCode}: {Detail} in {ElapsedMilliseconds}ms",
                    context.Method,
                    ex.StatusCode,
                    ex.Status.Detail,
                    stopwatch.ElapsedMilliseconds);
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "gRPC call failed for {GrpcMethod} in {ElapsedMilliseconds}ms",
                    context.Method,
                    stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }

    private static bool IsExpectedClientError(StatusCode statusCode) =>
        statusCode is StatusCode.InvalidArgument
            or StatusCode.NotFound
            or StatusCode.AlreadyExists
            or StatusCode.Aborted
            or StatusCode.FailedPrecondition;
}
