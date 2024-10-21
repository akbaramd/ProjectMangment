using AutoMapper;
using PMS.Application.DTOs;
using PMS.Domain.Entities;

namespace PMS.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Map User to UserDto
            CreateMap<ApplicationUser, UserProfileDto>();
            CreateMap<RoleWithPermissionsDto, TenantRole>().ReverseMap();
            CreateMap<PermissionDto, ApplicationPermission>().ReverseMap();
            CreateMap<PermissionGroupDto, ApplicationPermissionGroup>().ReverseMap();
        }
    }
}