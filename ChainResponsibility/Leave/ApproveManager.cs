using ChainResponsibility.Entities;

namespace ChainResponsibility.Leave;

public class ApproveManager : ApproveSupervisorHandler
{
    public override List<PositionLevel> GetApproveSupervisor(ApplicationLeave applicationLeave)
    {
        int applicationDays = (applicationLeave.Leave.EndTime - applicationLeave.Leave.StartTime).Days + 1;

        if (applicationLeave.Employee.JobGrade == PositionLevel.Staff && 
            applicationDays >= 1 && applicationDays < 5)
        {
            Console.WriteLine($"Staff 1 ~ 4天以下由經理審核");
            return new List<PositionLevel>() 
            {
                PositionLevel.Manager
            };
        }

        if (applicationLeave.Employee.JobGrade == PositionLevel.Manager &&
            applicationDays >= 1 && applicationDays < 6)
        {
            Console.WriteLine($"Manager 1 ~ 6天以下由協理審核");
            return new List<PositionLevel>()
            {
                PositionLevel.Director
            };
        }

        return base.GetApproveSupervisor(applicationLeave);
    }
}
