using PMS.Domain.BoundedContexts.TenantManagment;

namespace PMS.Application.DTOs
{
    public class TenantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Subdomain { get; set; }
        public TenantrStatus Status { get; set; }
        public List<TenantMemberDto>? Members { get; set; }
    }

    public class TenantMemberDto
    {
        public Guid UserId { get; set; }
        public UserProfileDto User { get; set; }
        public List<RoleWithPermissionsDto> Roles { get; set; }
        public TenantMemberStatus Status { get; set; }
    }

    public class TenantMemberUpdate
    {
        public Guid Role { get; set; }
    }

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
}