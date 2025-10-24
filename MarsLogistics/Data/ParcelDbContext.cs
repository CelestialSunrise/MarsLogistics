using MarsLogistics.Models;
using Microsoft.EntityFrameworkCore;

namespace MarsLogistics.Data
{
    public class ParcelDbContext : DbContext
    {
        public ParcelDbContext(DbContextOptions<ParcelDbContext> options) : base(options) { }   

        public DbSet<Parcel> Parcels { get; set; }
    }
}
