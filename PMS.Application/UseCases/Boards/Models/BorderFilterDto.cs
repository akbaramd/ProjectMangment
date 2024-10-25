namespace PMS.Application.UseCases.Boards.Models;

public class BorderFilterDto
{
  

    public int Take { get; set; }
    public int Skip { get; set; }
    public string? Search { get; set; }
    public Guid? SprintId { get; set; }
    
}