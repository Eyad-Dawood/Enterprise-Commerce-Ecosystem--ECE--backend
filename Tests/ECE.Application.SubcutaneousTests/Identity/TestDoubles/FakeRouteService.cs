namespace ECE.Application.SubcutaneousTests.Identity.TestDoubles;

using ECE.Application.Common.Interfaces;

internal sealed class FakeRouteService : IRouteService
{
    public string GetEmailConfirmationRoute(string email, string token)
        => $"https://example.test/confirm?email={email}&token={token}";

    public string GetResetPasswordRoute(string email, string token)
        => $"https://example.test/reset?email={email}&token={token}";
}

