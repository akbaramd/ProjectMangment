namespace PMS.Application.DTOs;

public class CreateProjectDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
}

public class UpdateProjectDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? EndDate { get; set; }
}

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid TenantId { get; set; }
}

public class ProjectDetailDto : ProjectDto
{
    public List<SprintDto> Sprints { get; set; }
}



public class BoardDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<BoardColumnDto> Columns { get; set; }
}

public class BoardColumnDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public List<TaskDto> Tasks { get; set; } // Optionally include tasks in the column
}

public class SprintDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<TaskDto> Tasks { get; set; }
    public List<BoardDto> Boards { get; set; }
}

public class TaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TaskStatus Status { get; set; }
    public int Order { get; set; }
}