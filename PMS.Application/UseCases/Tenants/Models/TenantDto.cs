namespace PMS.Application.UseCases.Tenants.Models
{
    public class TenantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Subdomain { get; set; }
        public List<TenantMemberDto>? Members { get; set; }
    }
}