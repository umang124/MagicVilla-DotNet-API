using MagicVillaAPI.Models;

namespace MagicVillaAPI.Repository.IRepository
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        Task UpdateAsync(VillaNumber entity);
    }
}
