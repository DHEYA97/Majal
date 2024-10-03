namespace Majal.Core.Contract.Client
{
    public record ClientRequest(
        int? Id,
        string Name,
        string? Url
       );
}
