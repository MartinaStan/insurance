using Claims.Auditing;
using Claims.Repositories;
using Claims.Services;
using Claims.Validation;
using Moq;
using Xunit;

namespace Claims.Tests
{
    public class ClaimsServiceTests
    {
        [Fact]
        public async Task CreateClaimAsync_GeneratesId_AndCallsRepositoryAndAuditer()
        {
            // Arrange
            var mockClaimsRepository = new Mock<IClaimsRepository>();
            var mockCoversRepository = new Mock<ICoversRepository>();
            var mockAuditer = new Mock<IAuditer>();

            var cover = new Cover
            {
                Id = "cover-1",
                StartDate = new DateTime(2026, 1, 1),
                EndDate = new DateTime(2026, 12, 31)
            };

            mockCoversRepository
                .Setup(repo => repo.GetCoverAsync("cover-1"))
                .ReturnsAsync(cover);

            var service = new ClaimsService(mockClaimsRepository.Object, mockAuditer.Object, mockCoversRepository.Object);

            var claim = new Claim
            {
                CoverId = "cover-1",
                Created = new DateTime(2026, 6, 1),
                DamageCost = 5000
            };

            // Act
            var result = await service.CreateClaimAsync(claim);

            // Assert
            Assert.False(string.IsNullOrEmpty(result.Id));
            mockClaimsRepository.Verify(repo => repo.AddItemAsync(claim), Times.Once);
            mockAuditer.Verify(auditer => auditer.AuditClaim(result.Id, "POST"), Times.Once);


        }
        [Fact]
        public async Task CreateClaimAsync_DamageCostExceedsLimit_ThrowsValidationException()
        {
            // Arrange
            var mockClaimsRepository = new Mock<IClaimsRepository>();
            var mockCoversRepository = new Mock<ICoversRepository>();
            var mockAuditer = new Mock<IAuditer>();

            var cover = new Cover
            {
                Id = "cover-1",
                StartDate = new DateTime(2026, 1, 1),
                EndDate = new DateTime(2026, 12, 31)
            };

            mockCoversRepository
                .Setup(repo => repo.GetCoverAsync("cover-1"))
                .ReturnsAsync(cover);

            var service = new ClaimsService(mockClaimsRepository.Object, mockAuditer.Object, mockCoversRepository.Object);

            var claim = new Claim
            {
                CoverId = "cover-1",
                Created = new DateTime(2026, 6, 1),
                DamageCost = 150000  // exceeds the allowed limit
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(
                () => service.CreateClaimAsync(claim));
        }
        [Fact]
        public async Task CreateClaimAsync_CreatedDateOutsideCoverPeriod_ThrowsValidationException()
        {
            // Arrange
            var mockClaimsRepository = new Mock<IClaimsRepository>();
            var mockCoversRepository = new Mock<ICoversRepository>();
            var mockAuditer = new Mock<IAuditer>();

            var cover = new Cover
            {
                Id = "cover-1",
                StartDate = new DateTime(2026, 1, 1),
                EndDate = new DateTime(2026, 12, 31)
            };

            mockCoversRepository
                .Setup(repo => repo.GetCoverAsync("cover-1"))
                .ReturnsAsync(cover);

            var service = new ClaimsService(mockClaimsRepository.Object, mockAuditer.Object, mockCoversRepository.Object);

            var claim = new Claim
            {
                CoverId = "cover-1",
                Created = new DateTime(2027, 3, 1),  // outside the cover period
                DamageCost = 5000
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(
                () => service.CreateClaimAsync(claim));
        }


    }

}