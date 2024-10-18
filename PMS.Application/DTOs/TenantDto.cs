
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
        public TenantMemberRole? CurrentUserRole { get; set; }
    }
    
    public class TenantMemberDto
    {
        public Guid UserId { get; set; }
        public UserProfileDto User { get; set; }
        public TenantMemberRole MemberRole { get; set; }
        public TenantMemberStatus MemberStatus { get; set; }
    }
    
    public class TenantMemberUpdate
    {
        public TenantMemberRole Role { get; set; }
    }
}
