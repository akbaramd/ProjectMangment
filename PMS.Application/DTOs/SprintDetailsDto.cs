namespace PMS.Application.DTOs;


public class SprintFilterDto
{
  

    public int Take { get; set; }
    public int Skip { get; set; }
    public string? Search { get; set; }
    public Guid? ProjectId { get; set; }
    
}
public class SprintDetailsDto : SprintDto
{
    public List<BoardDto> Boards { get; set; }
}


public class SprintDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

}


public class CreateSprintDto
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid ProjectId { get; set; }
}

public class UpdateSprintDto
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}