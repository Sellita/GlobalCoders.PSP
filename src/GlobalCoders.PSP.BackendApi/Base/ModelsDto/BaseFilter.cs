using System.ComponentModel.DataAnnotations;
using GlobalCoders.PSP.BackendApi.Base.Constants;

namespace GlobalCoders.PSP.BackendApi.Base.ModelsDto;

public class BaseFilter
{
    [Range(PaginationConstants.FirstPage, int.MaxValue)]
    public int Page { get; set; }
    [Range(PaginationConstants.MinimumItemsPerPage, PaginationConstants.MaximumItemsPerPage)]
    public int ItemsPerPage { get; set; }
}