namespace Majal.Core.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<EmployeeDepartment> Employees { get; set; }
    }
}
