namespace PMS.Application.UseCases.Tenant.Models;

public class TenantMembersFilterDto
{
    public TenantMembersFilterDto(int take, int skip, string? search, string orderBy, string orderDirection)
    {
        Take = take;
        Skip = skip;
        Search = search;
        SortDirection = orderDirection;
        SortBy = orderBy;
    }

    public int Take { get; set; }
    public int Skip { get; set; }
    public string? Search { get; set; }
    public string SortBy { get; set; }
    public string SortDirection { get; set; }
}