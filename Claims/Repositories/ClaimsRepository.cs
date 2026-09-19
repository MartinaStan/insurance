using Claims.Data;
using Microsoft.EntityFrameworkCore;

namespace Claims.Repositories
{
    /// <summary> EF Core / MongoDB implementation of <see cref="IClaimsRepository"/> </summary>
    public class ClaimsRepository : IClaimsRepository
    {
        private readonly ClaimsContext _context;

        public ClaimsRepository(ClaimsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            return await _context.Claims.ToListAsync();
        }

        public async Task<Claim> GetClaimAsync(string id)
        {
            return await _context.Claims
                .Where(claim => claim.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task AddItemAsync(Claim item)
        {
            _context.Claims.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(string id)
        {
            var claim = await GetClaimAsync(id);
            if (claim is not null)
            {
                _context.Claims.Remove(claim);
                await _context.SaveChangesAsync();
            }
        }
    }
}