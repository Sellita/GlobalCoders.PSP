namespace GlobalCoders.PSP.BackendApi.OrganizationManagment.ModelsDto;

public class OrganizationStatisticRequest
{
    public Guid OrganizationId { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}