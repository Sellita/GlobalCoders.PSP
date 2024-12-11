namespace GlobalCoders.PSP.BackendApi.Base.ModelsDto;

public class BasePagedResponse<TModel>
{
    public List<TModel> Items { get; set; } = new();
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
}