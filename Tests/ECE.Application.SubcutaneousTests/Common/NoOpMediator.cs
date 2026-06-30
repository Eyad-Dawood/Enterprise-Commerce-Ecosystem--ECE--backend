using MediatR;
using System.Runtime.CompilerServices;

namespace ECE.Application.SubcutaneousTests.Common;

public sealed class NoOpMediator : IMediator
{
    public Task Publish(object notification, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
        => Task.CompletedTask;

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        => Task.FromResult(default(TResponse)!);

    public Task<object?> Send(object request, CancellationToken cancellationToken = default)
        => Task.FromResult<object?>(null);

    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest
        => Task.CompletedTask;

    public async IAsyncEnumerable<TResponse> CreateStream<TResponse>(
        IStreamRequest<TResponse> request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        yield break;
    }

    public async IAsyncEnumerable<object?> CreateStream(
        object request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        yield break;
    }
}
