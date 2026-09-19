namespace Claims.Services
{
    /// <summary> Provides business operations for managing insurance claims</summary>
    public interface IClaimsService
    {
        Task<IEnumerable<Claim>> GetClaimsAsync();

        Task<Claim> GetClaimAsync(string id);

        Task<Claim> CreateClaimAsync(Claim claim);

        Task DeleteClaimAsync(string id);
    }
}