namespace ECE.Domain.Common.Results.Abstractions;

public interface IErrorFactory<TSelf>
{
    static abstract TSelf FromErrors(List<Error> errors);
}
