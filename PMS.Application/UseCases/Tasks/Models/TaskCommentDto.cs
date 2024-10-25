using PMS.Application.UseCases.Projects.Models;

namespace PMS.Application.UseCases.Tasks.Models;

public class TaskCommentDto
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public string Content { get; set; }
    public ProjectMemberDto ProjectMember { get; set; }
    
    public DateTime CreatedAt { get; set; }
}

public class TaskCommentCreateDto
{
    public string Content { get; set; }
}

public class TaskCommentUpdateDto
{
    public string Content { get; set; }
}