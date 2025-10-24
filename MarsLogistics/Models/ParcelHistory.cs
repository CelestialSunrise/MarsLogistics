using Microsoft.EntityFrameworkCore;

namespace MarsLogistics.Models
{
    [Owned]
    public class ParcelHistory
    {
        public ParcelStatus Status { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
