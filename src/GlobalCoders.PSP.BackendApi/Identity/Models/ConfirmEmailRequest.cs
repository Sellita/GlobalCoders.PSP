namespace GlobalCoders.PSP.BackendApi.Identity.Models;

public sealed class ConfirmEmailRequest
{
    public required Guid UserId { get; set; }
    public required string Code { get; set; }
    public string? ChangedEmail { get; set; }
}
