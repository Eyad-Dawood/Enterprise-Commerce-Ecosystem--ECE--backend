namespace ECE.Application.Features.Identity.Dtos;

public record ResetPasswordTokenDto(
    string Email,
    string EncoddedToken);
