using Majal.Core.Entities.Identity;
using Majal.Core.Contract.Auth;
using Majal.Core.Contract.Auth.User;
using Majal.Core.Contract.Client;
using Majal.Core.Contract.OurSystem;
using Majal.Core.Contract.Post;

namespace Majal.Api.Mapping
{
    public class MappingConfiguration() : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Client, ClientResponse>()
                  .Map(des => des.Url, src => src.Image.Url);

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


            config.NewConfig<OurSystem, OurSystemsResponse>()
                  .Map(des => des.MainImage, src => src.Image.Url);

            config.NewConfig<OurSystem, OurSystemResponse>()
                  .Map(des => des.MainImage, src => src.Image.Url)
                  .Map(des => des.SystemImages, src => src.SystemImages.Select(x => x.Image.Url))
                  .Map(des => des.Features, src => src.Features.Select(x => x.Content));

            config.NewConfig<OurSystemRequest, OurSystem>()
                 .Ignore(x => x.HasDemo)
                 .Ignore(x => x.Features)
                 .Ignore(x => x.SystemImages);


            config.NewConfig<Post, PostResponse>()
                  .Map(des => des.PostCategory, src => src.PostCategory.Name)
                  .Map(des => des.Url, src => src.Image.Url)
                  .Map(des => des.CreatedBy, src => $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}");

        }
    }
}
