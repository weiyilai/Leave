using ChainResponsibility.Entities;

namespace ChainResponsibility.Leave;

public class ApproveDirector : ApproveSupervisorHandler
{
    public override List<PositionLevel> GetApproveSupervisor(ApplicationLeave applicationLeave)
    {
        int applicationDays = (applicationLeave.Leave.EndTime - applicationLeave.Leave.StartTime).Days + 1;

        if (applicationLeave.Employee.JobGrade == PositionLevel.Staff &&
            applicationDays >= 5)
        {
            Console.WriteLine($"Staff 5天以上簽核至協理審核");

            return new List<PositionLevel>()
            {
                PositionLevel.Manager,
                PositionLevel.Director
            };
        }

        if (applicationLeave.Employee.JobGrade == PositionLevel.Manager &&
            applicationDays >= 7)
        {
            Console.WriteLine($"Manager 7天以上簽核至總經理審核");

            return new List<PositionLevel>()
            {
                PositionLevel.Director,
                PositionLevel.GeneralManager
            };
        }

        return base.GetApproveSupervisor(applicationLeave);
    }
}
