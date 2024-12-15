using GlobalCoders.PSP.BackendApi.TaxManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.TaxManagement.Factories;

public static class TaxFilterFactory
{
    public static TaxFilter CreateForAllItems(Guid merchantId)
    {
        return new TaxFilter
        {
            MerchantId = merchantId,
            Page = 1,
            ItemsPerPage = int.MaxValue,
            
        };
    }
}