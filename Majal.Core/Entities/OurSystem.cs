namespace Majal.Core.Entities
{
    public class OurSystem : BaseEntity
    {
        public string Name { get; set; }
        public string MainContentMedia { get; set; }
        public string Content { get; set; }
        public bool HasDemo {  get; set; }
        public string? DemoUrl { get; set; }
        public int MainImageId { get; set; }
        public Image Image { get; set; }
        public ICollection<SystemImage> SystemImages { get; set; }
        public ICollection<Feature> Features { get; set; }
    }
}
