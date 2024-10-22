using PMS.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PMS.Domain.Entities
{
    // Entity representing a Project
    public class Project : TenantEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }

        private readonly List<ProjectMember> _members = new List<ProjectMember>();
        public ICollection<ProjectMember> Members => _members.AsReadOnly();

        private readonly List<Sprint> _sprints = new List<Sprint>();
        public ICollection<Sprint> Sprints => _sprints.AsReadOnly();

        protected Project()
        {
        }

        public Project(string name, string description, DateTime startDate, Tenant tenant)
            : base(tenant)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
        }

        // Add members to the project
        public void AddMember(ProjectMember member)
        {
            _members.Add(member);
        }

        public void RemoveMember(ProjectMember member)
        {
            _members.Remove(member);
        }

        // Sprint-related methods
        public void AddSprint(Sprint sprint)
        {
            _sprints.Add(sprint);
        }

        public void RemoveSprint(Sprint sprint)
        {
            _sprints.Remove(sprint);
        }

        
        // Method to update project details
        public void UpdateDetails(string name, string description, DateTime? endDate)
        {
            Name = name;
            Description = description;
            EndDate = endDate;
        }

        public void ChangeStartDate(DateTime newStartDate)
        {
            StartDate = newStartDate;
        }

        public void ChangeEndDate(DateTime? newEndDate)
        {
            EndDate = newEndDate;
        }
    }
}
