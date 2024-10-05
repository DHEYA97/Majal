using Majal.Core.Entities;


namespace Majal.Core.Specification.EntitySpecification
{
    public class ClientSpecification : BaseSpecification<Client>
    {
        public ClientSpecification()
        {
            Includes.Add(i => i.Image);
        }

        public ClientSpecification(int? id) : base(x=>x.Id == id)
        {
            Includes.Add(i => i.Image);
        }
    }
}