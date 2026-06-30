namespace ECE.Application.Common.Interfaces;

public interface IRouteParameterService
{
    Result<string> GetRouteParameter(string parameterName);
}
