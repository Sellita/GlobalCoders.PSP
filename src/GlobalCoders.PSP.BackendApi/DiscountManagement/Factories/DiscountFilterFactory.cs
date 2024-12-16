using GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.Factories;

public static class DiscountFilterFactory
{
    public static DiscountFilter CreateForAllItems(Guid merchantId)
    {
        return new DiscountFilter
        {
            MerchantId = merchantId,
            Page = 1,
            ItemsPerPage = int.MaxValue,
            Date = DateTime.UtcNow
        };
    }
}