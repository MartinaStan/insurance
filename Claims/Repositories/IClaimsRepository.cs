namespace Claims.Repositories
{
    /// <summary>Provides data access operations for claims </summary>
    public interface IClaimsRepository
    {
        Task<IEnumerable<Claim>> GetClaimsAsync();

        Task<Claim> GetClaimAsync(string id);

        Task AddItemAsync(Claim item);

        Task DeleteItemAsync(string id);
    }
}