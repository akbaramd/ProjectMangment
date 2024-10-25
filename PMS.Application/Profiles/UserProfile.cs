using AutoMapper;
using PMS.Application.DTOs;
using PMS.Domain.BoundedContexts.TenantManagment;
using PMS.Domain.BoundedContexts.UserManagment;

namespace PMS.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Map User to UserDto
            CreateMap<ApplicationUser, UserProfileDto>();
            CreateMap<RoleWithPermissionsDto, TenantRoleEntity>().ReverseMap();
            CreateMap<PermissionDto, TenantPermissionEntity>().ReverseMap();
            CreateMap<PermissionGroupDto, TenantPermissionGroupEntity>().ReverseMap();
        }
    }
}