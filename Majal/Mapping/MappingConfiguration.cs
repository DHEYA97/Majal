using Majal.Core.Entities.Identity;
using Majal.Core.Contract.Auth;
using Majal.Core.Contract.Auth.User;

namespace Majal.Api.Mapping
{
    public class MappingConfiguration() : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Employee, EmployeeResponse>()
                  .Map(des => des.Departments, src => src.Departments.Select(x => x.Department.Name));

            config.NewConfig<RegisterRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email);

            config.NewConfig<(ApplicationUser user, IList<string> role), UserDetailsResponse>()
            .Map(dest => dest, src => src.user)
            .Map(dest => dest.Roles, src => src.role);

            config.NewConfig<AddUserRequest, ApplicationUser>()
                .Map(des => des.UserName, src => src.Email)
                .Map(des => des.EmailConfirmed, src => true);

            config.NewConfig<UpdateUserRequest, ApplicationUser>()
                .Map(des => des.UserName, src => src.Email)
                .Map(des => des.NormalizedUserName, src => src.Email.ToUpper());
        }
    }
}
