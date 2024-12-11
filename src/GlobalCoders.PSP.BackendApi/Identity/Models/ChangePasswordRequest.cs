namespace GlobalCoders.PSP.BackendApi.Identity.Models;

public sealed class ChangePasswordRequest
{
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}
