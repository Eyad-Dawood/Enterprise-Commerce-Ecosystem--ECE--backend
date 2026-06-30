namespace ECE.Application.Features.Identity.Commands.SendEmailConfirmation;

public record SendEmailConfirmationCommand(string Email) : IRequest<Result<Success>>;
