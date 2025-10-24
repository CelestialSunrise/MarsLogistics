using System.ComponentModel.DataAnnotations;

namespace MarsLogistics.Models
{
    public class Parcel
    {
        [Key]
        public string Barcode { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string Recipient { get; set; } = string.Empty;
        public string DeliveryService { get; set; } = string.Empty;
        public string Contents { get; set; } = string.Empty;
        public ParcelStatus Status { get; set; } = ParcelStatus.Created;
        public string Origin { get; set; } = "Starport Thames Estuary";
        public string Destination { get; set; } = "New London";
        public DateTime LaunchDate { get; set; }
        public int EtaDays { get; set; }
        public DateTime EstimatedArrivalDate { get; set; }

        public List<ParcelHistory> History { get; set; } = new();
    }
}
