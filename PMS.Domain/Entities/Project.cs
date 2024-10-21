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

        private readonly List<Sprint> _sprints = new List<Sprint>();
        public IReadOnlyCollection<Sprint> Sprints => _sprints.AsReadOnly();

        protected Project() { }

        public Project(string name, string description, DateTime startDate, Tenant tenant)
            : base(tenant)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            
            
        }

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

    // Entity representing a Sprint

    // Entity representing a Task

    // Enum to represent the status of a Task

    // Entity representing a Board (Kanban board)

    // Entity representing a column in a Board
}
