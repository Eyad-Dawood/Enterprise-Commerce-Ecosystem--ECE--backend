namespace ECE.Application.SubcutaneousTests.Identity.TestDoubles;

using ECE.Application.Common.Interfaces;
using ECE.Domain.Common.Results;

internal sealed class FakeEmailFormatService : IEmailFormatService
{
    public Result<string> ConfirmEmailResult { get; set; } = "confirm-body";
    public Result<string> ResetPasswordResult { get; set; } = "reset-body";

    public Result<string> ConfirmEmailFormat(string link)
        => ConfirmEmailResult;

    public Result<string> ResetPasswordEmailFormat(string link)
        => ResetPasswordResult;
}

