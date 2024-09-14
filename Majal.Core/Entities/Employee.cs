namespace Majal.Core.Entities
{
    public class Employee : BaseEntity
    {
        public string Name { get; set; }
        public string DateOfBarth { get; set; }
        public ICollection<EmployeeDepartment> Departments { get; set; }
    }
}
