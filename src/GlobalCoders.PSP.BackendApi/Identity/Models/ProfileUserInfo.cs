namespace GlobalCoders.PSP.BackendApi.Identity.Models;

public sealed class ProfileUserInfo
{
    public required Guid UserId { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    
    public string? MerchantName {get; set;}
    public Guid? MerchantId {get; set;}
}
