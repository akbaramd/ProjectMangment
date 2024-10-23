
using PMS.Domain.Entities;

namespace PMS.Application.DTOs
{
    public class TenantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Subdomain { get; set; }
        public TenantStatus Status { get; set; }
        public List<TenantMemberDto>? Members { get; set; }
    }
    
    public class TenantMemberDto
    {
        public Guid UserId { get; set; }
        public UserProfileDto User { get; set; }
        public List<RoleWithPermissionsDto> Roles { get; set; }
        public TenantMemberStatus MemberStatus { get; set; }
    }
    
    public class TenantMemberUpdate
    {
        public Guid Role { get; set; }
    }
    
    public class TenantMembersFilterDto
    {
        public TenantMembersFilterDto(int take, int skip, string? search)
        {
            Take = take;
            Skip = skip;
            Search = search;
        }

        public int Take { get; set; }
        public int Skip { get; set; }
        public string? Search { get; set; }
    
    
    
    }
}
