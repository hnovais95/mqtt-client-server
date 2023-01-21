using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Domain
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public Task<IEnumerable<TEntity>> GetAll();
        public void Insert(TEntity entity, int timeout = 0);
    }
}
