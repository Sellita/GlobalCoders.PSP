namespace GlobalCoders.PSP.BackendApi.DiscountManagment.Extensions;

public static class DataExtension
{
    public static bool IsValidDateRange(this DateTime startDate, DateTime endDate)
    {
        return startDate < endDate && startDate > DateTime.UtcNow;
    }
}