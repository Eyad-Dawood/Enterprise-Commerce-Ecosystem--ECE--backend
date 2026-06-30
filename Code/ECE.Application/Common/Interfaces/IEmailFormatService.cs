namespace ECE.Application.Common.Interfaces;

public interface IEmailFormatService
{
    Result<string> ConfirmEmailFormat(string link);
    Result<string> ResetPasswordEmailFormat(string link);
}
