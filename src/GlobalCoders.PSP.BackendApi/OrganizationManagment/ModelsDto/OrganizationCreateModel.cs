namespace GlobalCoders.PSP.BackendApi.OrganizationManagment.ModelsDto;

public class OrganizationCreateModel
{
    public string DisplayName { get; set; } = string.Empty;
    public string LegalName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MainPhoneNumber { get; set; } = string.Empty;
    public string SecondaryPhoneNumber { get; set; } = string.Empty;
    public List<OrganizationScheduleRequest> WorkingSchedule { get; set; } = new ();
}