namespace Majal.Core.Entities
{
    public enum ImageType
    {
        System,
        Client,
        Post
    }
    public class Image : BaseEntity
    {
        public string Url { get; set; }
        public ImageType Type { get; set; }
    }
}
