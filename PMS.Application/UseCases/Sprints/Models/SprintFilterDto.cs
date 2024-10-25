namespace PMS.Application.UseCases.Sprints.Model;

public class SprintFilterDto 
{
    public int Take { get; set; }
    public int Skip { get; set; }
    public string? Search { get; set; }
    public Guid? ProjectId { get; set; }
    
}