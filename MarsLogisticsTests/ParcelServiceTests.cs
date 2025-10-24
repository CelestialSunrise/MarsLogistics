using MarsLogistics.Data;
using MarsLogistics.Models;
using MarsLogistics.Services;
using Microsoft.EntityFrameworkCore;

namespace MarsLogisticsTests
{
    public class ParcelServiceTests
    {
        private ParcelDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ParcelDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ParcelDbContext(options);
        }

        [Fact]
        public async Task RegisterParcelAsync_ShouldSetLaunchDateAndStatus()
        {
            var context = GetDbContext();
            var service = new ParcelService(context);

            var parcel = new Parcel
            {
                Barcode = "RMARS1234567890123456789M",
                Sender = "Amit",
                Recipient = "Mars Base",
                DeliveryService = "Express",
                Contents = "Test parcel"
            };

            var result = await service.RegisterParcelAsync(parcel);

            Assert.Equal(ParcelStatus.Created, result.Status);
            Assert.True(result.LaunchDate > DateTime.MinValue);
            Assert.Equal(1, result.History.Count);
            Assert.Equal(ParcelStatus.Created, result.History[0].Status);
        }

        [Fact]
        public async Task GetParcelAsync_ShouldReturnParcel_WhenExists()
        {
            var context = GetDbContext();
            var service = new ParcelService(context);

            var parcel = new Parcel
            {
                Barcode = "RMARS1234567890123456789M",
                Sender = "Amit",
                Recipient = "Mars Base",
                DeliveryService = "Express",
                Contents = "Test parcel"
            };

            await service.RegisterParcelAsync(parcel);

            var fetched = await service.GetParcelAsync(parcel.Barcode);

            Assert.NotNull(fetched);
            Assert.Equal(parcel.Barcode, fetched.Barcode);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldUpdateStatus_WhenValid()
        {
            var context = GetDbContext();
            var service = new ParcelService(context);

            var parcel = new Parcel
            {
                Barcode = "RMARS1234567890123456789M",
                Sender = "Amit",
                Recipient = "Mars Base",
                DeliveryService = "Express",
                Contents = "Test parcel"
            };

            await service.RegisterParcelAsync(parcel);

            var updated = await service.UpdateStatusAsync(parcel.Barcode, ParcelStatus.OnRocketToMars);

            Assert.Equal(ParcelStatus.OnRocketToMars, updated.Status);
            Assert.Contains(updated.History, h => h.Status == ParcelStatus.OnRocketToMars);
        }
    }
}