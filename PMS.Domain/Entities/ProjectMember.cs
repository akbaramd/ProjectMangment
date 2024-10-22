using PMS.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.Entities
{
    // Entity representing a Project
    public class ProjectMember : TenantEntity
    {
        protected ProjectMember(){}


        public ProjectMember(Tenant tenant, TenantMember tenantMember, Project project, string access) : base(tenant)
        {
            TenantMember = tenantMember;
            Project = project;
            Access = Enumeration.FromName<ProjectMemberAccess>(access);;
        }

        public TenantMember TenantMember { get; set; }
        public Guid TenantMemberId { get; set; }
        
        public Project Project { get; set; }
        public Guid ProjectId { get; set; }

        public ProjectMemberAccess Access { get; set; }

        public void UpdateDetails(string access)
        {
            Access = Enumeration.FromName<ProjectMemberAccess>(access);
        }
    }


    public class ProjectMemberAccess : Enumeration
    {
        public static ProjectMemberAccess ProductOwner = new ProjectMemberAccess(0, nameof(ProductOwner)); 
        public static ProjectMemberAccess ScrumMaster = new ProjectMemberAccess(1, nameof(ScrumMaster)); 
        public static ProjectMemberAccess Maintainer = new ProjectMemberAccess(2, nameof(Maintainer)); 
        public static ProjectMemberAccess Quest = new ProjectMemberAccess(3, nameof(Quest)); 
        public ProjectMemberAccess(int id, string name) : base(id, name)
        {
        }
    }
}
