using MarsLogistics.Models;

namespace MarsLogistics.Services.Interfaces
{
    public interface IParcelService
    {
        Task<Parcel> RegisterParcelAsync(Parcel parcel);
        Task<Parcel?> GetParcelAsync(string barcode);
        Task<Parcel?> UpdateStatusAsync(string barcode, ParcelStatus newStatus);
    }
}
