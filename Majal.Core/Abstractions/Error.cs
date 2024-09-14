namespace Majal.Core.Abstractions
{
    public record Error(string Code,string Description,int? status)
    {
        public static readonly Error None = new(string.Empty, string.Empty,null);
    }
}
