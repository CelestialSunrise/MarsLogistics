using MarsLogistics.Data;
using MarsLogistics.Models;
using MarsLogistics.Services.Interfaces;
using MarsLogistics.Utils;
using Microsoft.EntityFrameworkCore;

namespace MarsLogistics.Services
{
    public class ParcelService : IParcelService
    {
        private readonly ParcelDbContext _context;

        public ParcelService(ParcelDbContext context)
        {
            _context = context;
        }

        public async Task<Parcel> RegisterParcelAsync(Parcel parcel)
        {
            if (!BarcodeValidator.IsValid(parcel.Barcode))
                throw new ArgumentException("Invalid barcode format.");

            (DateTime launchDate, int etaDays) = LaunchScheduler.GetLaunchInfo(parcel.DeliveryService);
            parcel.LaunchDate = launchDate;
            parcel.EtaDays = etaDays;
            parcel.EstimatedArrivalDate = launchDate.AddDays(etaDays);
            parcel.Status = ParcelStatus.Created;
            parcel.History.Add(new ParcelHistory { Status = ParcelStatus.Created, Timestamp = DateTime.UtcNow });

            _context.Parcels.Add(parcel);
            await _context.SaveChangesAsync();
            return parcel;
        }

        public async Task<Parcel?> GetParcelAsync(string barcode) =>
            await _context.Parcels.Include(p => p.History).FirstOrDefaultAsync(p => p.Barcode == barcode);

        public async Task<Parcel?> UpdateStatusAsync(string barcode, ParcelStatus newStatus)
        {
            var parcel = await _context.Parcels.Include(p => p.History).FirstOrDefaultAsync(p => p.Barcode == barcode);
            if (parcel is null) return null;

            if (!StatusTransitionValidator.IsValid(parcel, newStatus))
                throw new InvalidOperationException("Invalid status transition.");

            parcel.Status = newStatus;
            parcel.History.Add(new ParcelHistory { Status = newStatus, Timestamp = DateTime.UtcNow });

            await _context.SaveChangesAsync();
            return parcel;
        }
    }

}