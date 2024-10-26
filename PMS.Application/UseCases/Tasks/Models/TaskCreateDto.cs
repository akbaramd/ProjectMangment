﻿namespace PMS.Application.UseCases.Tasks.Models;

public class TaskCreateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    
    public Guid ColumnId { get; set; }
    public int Order { get; set; }
    public DateTime? DueDate { get; set; }
}