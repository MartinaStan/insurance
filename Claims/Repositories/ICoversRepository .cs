using Claims;

namespace Covers.Repositories
{
    /// <summary>Provides data access operations for covers</summary>
    public interface ICoversRepository
    {
        Task<IEnumerable<Cover>> GetCoversAsync();

        Task<Cover> GetCoverAsync(string id);

        Task AddItemAsync(Cover item);

        Task DeleteItemAsync(string id);
    }
}