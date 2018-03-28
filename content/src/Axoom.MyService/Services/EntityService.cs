using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Axoom.MyService.Dto;

namespace Axoom.MyService.Services
{
    /// <summary>
    /// A sample service.
    /// </summary>
    public class EntityService : IEntityService
    {
        public Task<IEnumerable<Entity>> GetAllAsync()
            => Task.FromResult(new[]
            {
                new Entity {Id = "1", Data = "a"},
                new Entity {Id = "2", Data = "b"}
            }.AsEnumerable());

        public Task<Entity> GetAsync(string id)
            => Task.FromResult(new Entity {Id = id, Data = "x"});

        public Task<Entity> AddAsync(Entity entity)
            => Task.FromResult(new Entity {Id = "999", Data = entity.Data});

        public Task ModifyAsync(Entity entity)
            => Task.CompletedTask;

        public Task RemoveAsync(string id)
            => Task.CompletedTask;
    }
}
