

namespace Majal.Core.Entities
{
    public class SystemImage
    {
        public int OurSystemId { get; set; }
        public int ImageId {  get; set; }
        public OurSystem OurSystem { get; set; }
        public Image Image { get; set; }
    }
}
