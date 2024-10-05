namespace Majal.Core.Contract.Post
{
    public record PostRequest(
        int? Id,
        string Title,
        string Body,

        string PostCategory,
        string? Url
       );
}
