using PMS.Application.UseCases.Boards.Models;
using PMS.Application.UseCases.Projects.Models;

namespace PMS.Application.UseCases.Tasks.Models;
public class TaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public int Order { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public BoardColumnDto BoardColumn { get; set; }
    public ICollection<TaskCommentDto> Comments { get; set; }
    public ICollection<ProjectMemberDto> AssigneeMembers { get; set; }
}