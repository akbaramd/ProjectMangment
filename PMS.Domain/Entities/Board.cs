using PMS.Domain.Core;

namespace PMS.Domain.Entities;

public class Board : TenantEntity
{
    public string Name { get; private set; }
    private readonly List<BoardColumn> _columns = new List<BoardColumn>();
    public IReadOnlyCollection<BoardColumn> Columns => _columns.AsReadOnly();

    protected Board() { }
    public Guid SprintId { get; private set; }
    public Sprint Sprint { get; private set; }
    public Board(string name,Sprint sprint, Tenant tenant)
        : base(tenant)
    {
        Name = name;
        Sprint = sprint;
        InitializeDefaultColumns();
    }

    private void InitializeDefaultColumns()
    {
        _columns.Add(new BoardColumn("ToDo", 1,Tenant));
        _columns.Add(new BoardColumn("Doing", 2,Tenant));
        _columns.Add(new BoardColumn("Done", 3,Tenant));
    }

    public void AddColumn(BoardColumn column)
    {
        _columns.Add(column);
    }

    public void UpdateDetails(string name)
    {
        Name = name;
    }
}