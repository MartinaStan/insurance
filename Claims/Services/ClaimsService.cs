using Claims.Auditing;
using Claims.Repositories;
using Claims.Validation;

namespace Claims.Services
{
    /// <summary> Business logic for managing insurance claims</summary>
    public class ClaimsService : IClaimsService
    {
        private readonly IClaimsRepository _claimsRepository;
        private readonly IAuditer _auditer;
        private readonly ICoversRepository _coversRepository;

        public ClaimsService(IClaimsRepository claimsRepository, IAuditer auditer, ICoversRepository coversRepository)
        {
            _claimsRepository = claimsRepository;
            _auditer = auditer;
            _coversRepository = coversRepository;
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            return await _claimsRepository.GetClaimsAsync();
        }

        public async Task<Claim> GetClaimAsync(string id)
        {
            return await _claimsRepository.GetClaimAsync(id);
        }

        public async Task<Claim> CreateClaimAsync(Claim claim)
        {
            await ValidateClaimAsync(claim);
            claim.Id = Guid.NewGuid().ToString();
            await _claimsRepository.AddItemAsync(claim);
            _auditer.AuditClaim(claim.Id, "POST");
            return claim;
        }

        public async Task DeleteClaimAsync(string id)
        {
            _auditer.AuditClaim(id, "DELETE");
            await _claimsRepository.DeleteItemAsync(id);
        }
        private async Task ValidateClaimAsync(Claim claim)
        {
            if (claim.DamageCost > 100000)
            {
                throw new ValidationException("Damage cost cannot exceed 100,000!");
            }

            var cover = await _coversRepository.GetCoverAsync(claim.CoverId);
            if (cover is null)
            {
                throw new ValidationException("Cover was not found.");
            }

            if (claim.Created < cover.StartDate || claim.Created > cover.EndDate)
            {
                throw new ValidationException("Created date must be within the period of the related cover!");
            }
        }
    }
}