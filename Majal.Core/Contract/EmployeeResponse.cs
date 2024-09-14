namespace Majal.Core.Contract
{
  public record EmployeeResponse
    (   
        int Id,
        DateTime DateOfBarth,
        string Name,
        IList<string> Departments
    );
}
