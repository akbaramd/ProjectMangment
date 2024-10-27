using AutoMapper;
using PMS.Application.UseCases.Tenant.Models;
using PMS.Application.UseCases.User.Models;
using PMS.Domain.BoundedContexts.TenantManagement;
using PMS.Domain.BoundedContexts.UserManagment;

namespace PMS.Application.UseCases.User
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Map User to UserDto
            CreateMap<ApplicationUser, UserProfileDto>();
            CreateMap<TenantRoleDto, TenantRoleEntity>().ReverseMap();
            CreateMap<TenantPermissionDto, TenantPermissionEntity>().ReverseMap();
            CreateMap<TenantPermissionGroupDto, TenantPermissionGroupEntity>().ReverseMap();
        }
    }
}