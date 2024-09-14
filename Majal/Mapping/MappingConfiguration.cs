namespace Majal.Api.Mapping
{
    public class MappingConfiguration() : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Employee, EmployeeResponse>()
                  .Map(des => des.Departments, src => src.Departments.Select(x => x.Department.Name));
        }
    }
}
