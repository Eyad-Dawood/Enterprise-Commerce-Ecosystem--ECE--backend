namespace ECE.Application.SubcutaneousTests.Identity.TestDoubles;

using ECE.Application.Common.Interfaces;
using ECE.Application.Features.Identity.Dtos;
using ECE.Domain.Common.Results;
using System.Security.Claims;

internal sealed class FakeTokenProvider : ITokenProvider
{
    public Result<TokenDto> TokenResult { get; set; } = new TokenDto
    {
        AccessToken = "access",
        RefreshToken = "refresh",
        ExpiresOnUtc = DateTime.UtcNow.AddMinutes(15)
    };

    public ClaimsPrincipal? Principal { get; set; }

    public Task<Result<TokenDto>> GenerateJwtTokenAsync(AppUserDto user, CancellationToken ct)
        => Task.FromResult(TokenResult);

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string ExpiredAccessToken)
        => Principal;
}

