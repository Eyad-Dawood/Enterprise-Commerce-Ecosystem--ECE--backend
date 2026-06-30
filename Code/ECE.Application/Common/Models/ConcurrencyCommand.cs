namespace ECE.Application.Common.Models;

public record ConcurrencyCommand<TResponse>(string RowVersion) : IRequest<TResponse>;
