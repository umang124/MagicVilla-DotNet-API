using MagicVillaAPI.Data;
using MagicVillaAPI.Models;
using MagicVillaAPI.Repository.IRepository;

namespace MagicVillaAPI.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task UpdateAsync(Villa entity)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
