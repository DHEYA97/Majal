namespace Majal.Core.Entities
{
    public class OurSystem : BaseEntity
    {
        private bool _hasDemo;
        public string Name { get; set; }
        public string MainContentMedia { get; set; }
        public string Content { get; set; }
        public bool HasDemo
        {
            get
            {
                return !string.IsNullOrEmpty(DemoUrl);
            }
            set
            {
                _hasDemo = !string.IsNullOrEmpty(DemoUrl);
            }
        }
        public string? DemoUrl { get; set; }
        public int MainImageId { get; set; }
        public Image Image { get; set; }
        public ICollection<SystemImage> SystemImages { get; set; }
        public ICollection<Feature> Features { get; set; }
    }
}
