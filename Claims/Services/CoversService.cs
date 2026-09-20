using Claims.Auditing;
using Claims.Repositories;
using Claims.Validation;

namespace Claims.Services
{
    /// <summary> Business logic for managing insurance claims </summary>
    public class CoversService : ICoversService
    {
        private readonly ICoversRepository _coversRepository;
        private readonly IAuditer _auditer;

        public CoversService(ICoversRepository coversRepository, IAuditer auditer)
        {
            _coversRepository = coversRepository;
            _auditer = auditer;
        }

        public async Task<IEnumerable<Cover>> GetCoversAsync()
        {
            return await _coversRepository.GetCoversAsync();
        }

        public async Task<Cover> GetCoverAsync(string id)
        {
            return await _coversRepository.GetCoverAsync(id);

        }

        public async Task<Cover> CreateCoverAsync(Cover cover)
        {
            ValidateCover(cover);

            cover.Id = Guid.NewGuid().ToString();
            cover.Premium = ComputePremium(cover.StartDate, cover.EndDate, cover.Type);
            await _coversRepository.AddItemAsync(cover);
            _auditer.AuditCover(cover.Id, "POST");
            return cover;
        }

        /// <summary> Validates a cover before creation</summary>
        private void ValidateCover(Cover cover)
        {
            if (cover.StartDate.Date < DateTime.Now.Date)
            {
                throw new ValidationException("Start date cannot be in the past!");
            }

            var insurancePeriod = cover.EndDate - cover.StartDate;
            if (insurancePeriod.TotalDays > 365)
            {
                throw new ValidationException("Total insurance period cannot exceed 1 year!");
            }
        }

        public async Task DeleteCoverAsync(string id)
        {
            _auditer.AuditCover(id, "DELETE");
            await _coversRepository.DeleteItemAsync(id);
        }

        public decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
        {
            var multiplier = 1.3m;
            if (coverType == CoverType.Yacht)
            {
                multiplier = 1.1m;
            }

            if (coverType == CoverType.PassengerShip)
            {
                multiplier = 1.2m;
            }

            if (coverType == CoverType.Tanker)
            {
                multiplier = 1.5m;
            }

            var premiumPerDay = 1250 * multiplier;
            var insuranceLength = (endDate - startDate).TotalDays;
            var totalPremium = 0m;

            for (var i = 0; i < insuranceLength; i++)
            {
                if (i < 30)
                {
                    // First 30 days no discount
                    totalPremium += premiumPerDay;
                }
                else if (i < 180 && coverType == CoverType.Yacht)
                {
                    // Days 31–180, Yacht: 5% discount
                    totalPremium += premiumPerDay - premiumPerDay * 0.05m;
                }
                else if (i < 180)
                {
                    // Days 31–180, other types: 2% discount
                    totalPremium += premiumPerDay - premiumPerDay * 0.02m;
                }
                else if (i < 365 && coverType != CoverType.Yacht) 
                {
                    // Day 181 onward, other types: cumulative 3% discount
                    totalPremium += premiumPerDay - premiumPerDay * 0.03m;
                }
                else if (i < 365)
                {
                    // Day 181 onward, Yacht: cumulative 8% discount
                    totalPremium += premiumPerDay - premiumPerDay * 0.08m;
                }
            }
            return totalPremium;
        }
    }
}