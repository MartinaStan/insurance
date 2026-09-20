using Claims.Auditing;
using Claims.Repositories;
using Claims.Services;
using Claims.Validation;
using Moq;
using Xunit;

namespace Claims.Tests
{
    public class CoversServiceTests
    {
        [Fact]
        public async Task CreateCoverAsync_StartDateInThePast_ThrowsValidationException()
        {
            // Arrange
            var mockCoversRepository = new Mock<ICoversRepository>();
            var mockAuditer = new Mock<IAuditer>();

            var service = new CoversService(mockCoversRepository.Object, mockAuditer.Object);

            var cover = new Cover
            {
                StartDate = new DateTime(2020, 1, 1),  // in the past
                EndDate = new DateTime(2020, 6, 1),
                Type = CoverType.Yacht
            };

            // Act & Assert
            await Assert.ThrowsAsync<Claims.Validation.ValidationException>(
                () => service.CreateCoverAsync(cover));
        }

        [Fact]
        public async Task CreateCoverAsync_PeriodExceedsOneYear_ThrowsValidationException()
        {
            // Arrange
            var mockCoversRepository = new Mock<ICoversRepository>();
            var mockAuditer = new Mock<IAuditer>();

            var service = new CoversService(mockCoversRepository.Object, mockAuditer.Object);

            var cover = new Cover
            {
                StartDate = new DateTime(2026, 10, 1),
                EndDate = new DateTime(2028, 3, 1),  // exceeds 1 year
                Type = CoverType.Yacht
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(
                () => service.CreateCoverAsync(cover));
        }
    }
}