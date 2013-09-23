using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Projection;

namespace SocialToolBox.Core.Mocks.Database
{
    /// <summary>
    /// An in-memory implementation of a projection.
    /// </summary>
    public class Projection<T> : IProjection<T> where T : class
    {
        public string Name { get; private set; }
        
        public MultiProjector<T> Projector;

        public Projection(string name)
        {
            Name = name;
            Projector = new MultiProjector<T>(name);
        }

        public IEntityStore<TEn> Create<TEn>(string name, IEntityStoreProjection<T, TEn> proj, IEventStream[] streams) where TEn : class
        {
            throw new System.NotImplementedException();
        }

        public void Compile()
        {
            throw new System.NotImplementedException();
        }
    }
}
