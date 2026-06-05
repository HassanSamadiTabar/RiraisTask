using Grpc.Core;
using Grpc.Core.Interceptors;
using RiraisTask.Application.Exceptions;

namespace RiraisTask.Grpc.Interceptors;

public class ExceptionInterceptor(ILogger<ExceptionInterceptor> logger) : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (PersonValidationException ex)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }
        catch (PersonNotFoundException ex)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
        catch (PersonConflictException ex)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
        }
        catch (PersonConcurrencyException ex)
        {
            throw new RpcException(new Status(StatusCode.Aborted, ex.Message));
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled error in gRPC call {Method}", context.Method);
            throw new RpcException(new Status(StatusCode.Internal, "An unexpected error occurred."));
        }
    }
}
