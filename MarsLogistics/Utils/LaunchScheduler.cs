using MarsLogistics.Models;

namespace MarsLogistics.Utils
{
    public static class LaunchScheduler
    {
        public static (DateTime launchDate, int etaDays) GetLaunchInfo(string service)
        {
            var now = DateTime.UtcNow;

            if (service.Equals("Express", StringComparison.OrdinalIgnoreCase))
            {
                var firstWednesday = Enumerable.Range(1, 7)
                    .Select(day => new DateTime(now.Year, now.Month, day))
                    .First(d => d.DayOfWeek == DayOfWeek.Wednesday);

                if (now > firstWednesday)
                    firstWednesday = firstWednesday.AddMonths(1).AddDays(-(firstWednesday.Day - 1));

                return (firstWednesday, 90);
            }

            if (service.Equals("Standard", StringComparison.OrdinalIgnoreCase))
            {
                var nextLaunch = new DateTime(2025, 10, 1);
                return (nextLaunch, 180);
            }

            throw new ArgumentException("Unknown delivery service.");
        }
    }

    public static class BarcodeValidator
    {
        public static bool IsValid(string barcode) =>
            System.Text.RegularExpressions.Regex.IsMatch(barcode, @"^RMARS\d{19}[A-Z]$");
    }

    public static class StatusTransitionValidator
    {
        private static readonly Dictionary<ParcelStatus, ParcelStatus[]> ValidTransitions = new()
        {
            [ParcelStatus.Created] = new[] { ParcelStatus.OnRocketToMars },
            [ParcelStatus.OnRocketToMars] = new[] { ParcelStatus.LandedOnMars, ParcelStatus.Lost },
            [ParcelStatus.LandedOnMars] = new[] { ParcelStatus.OutForMartianDelivery },
            [ParcelStatus.OutForMartianDelivery] = new[] { ParcelStatus.Delivered, ParcelStatus.Lost }
        };

        public static bool IsValid(Parcel parcel, ParcelStatus newStatus)
        {
            if (parcel.Status == ParcelStatus.Delivered || parcel.Status == ParcelStatus.Lost)
                return false;

            return ValidTransitions.TryGetValue(parcel.Status, out var allowed) && allowed.Contains(newStatus);
        }
    }
}
