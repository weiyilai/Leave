namespace ChainResponsibility.Entities;

public class Employee
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime OnBoard { get; set; }
    public PositionLevel JobGrade { get; set; }
    public List<Leave> LeaveHistory { get; set; }
}
