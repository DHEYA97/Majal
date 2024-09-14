

namespace Majal.Core.Entities
{
    public class EmployeeDepartment
    {
        public int DepartmentId {  get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public Department Department { get; set; }
    }
}
