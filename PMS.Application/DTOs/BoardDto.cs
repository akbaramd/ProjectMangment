namespace PMS.Application.DTOs;
public class BorderFilterDto
{
  

    public int Take { get; set; }
    public int Skip { get; set; }
    public string? Search { get; set; }
    public Guid? SprintId { get; set; }
    
}
public class BoardDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<BoardColumnDto> Columns { get; set; }
}


public class CreateBoardDto
{
    public Guid SprintId { get; set; }
    public string Name { get; set; }
}

public class UpdateBoardDto
{
    public string Name { get; set; }
}

public class BoardColumnDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public List<TaskDto> Tasks { get; set; } // Optionally include tasks in the column
}

public class TaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TaskStatus Status { get; set; }
    public int Order { get; set; }
}