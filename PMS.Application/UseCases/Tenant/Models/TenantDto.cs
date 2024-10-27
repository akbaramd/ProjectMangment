using PMS.Domain.BoundedContexts.TenantManagement;

namespace PMS.Application.UseCases.Tenant.Models
{
    public class TenantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Subdomain { get; set; }
        public TenantrStatus Status { get; set; }
        public List<TenantMemberDto>? Members { get; set; }
    }
}