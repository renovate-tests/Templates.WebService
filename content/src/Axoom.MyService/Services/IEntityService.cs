using System.Collections.Generic;
using System.Threading.Tasks;
using Axoom.MyService.Dto;

namespace Axoom.MyService.Services
{
    /// <summary>
    /// A sample service.
    /// </summary>
    public interface IEntityService
    {
        Task<IEnumerable<Entity>> GetAllAsync();

        Task<Entity> GetAsync(string id);

        Task<Entity> AddAsync(Entity entity);

        Task ModifyAsync(Entity entity);

        Task RemoveAsync(string id);
    }
}