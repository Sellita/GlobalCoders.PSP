using GlobalCoders.PSP.BackendApi.Base.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.Base.Factories;

public static class BasePagedResopnseFactory
{
    public static BasePagedResponse<TModel> Create<TModel>(List<TModel> items, BaseFilter filter, int totalItems)
    {
        return new BasePagedResponse<TModel>
        {
            Items = items,
            Page = filter.Page,
            TotalPages = (int)Math.Ceiling((double)totalItems / filter.ItemsPerPage),
            TotalItems = totalItems
        };
    }   
    
    public static BasePagedResponse<TModel> CreateSingle<TModel>(TModel item)
    {
        return new BasePagedResponse<TModel>
        {
            Items = [item],
            Page = 1,
            TotalPages = 1,
            TotalItems = 1
        };
    }
}