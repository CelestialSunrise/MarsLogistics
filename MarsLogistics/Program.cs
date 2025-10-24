using System.Text.Json.Serialization;
using MarsLogistics.Data;
using MarsLogistics.Middleware;
using MarsLogistics.Models;
using MarsLogistics.Services;
using MarsLogistics.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarsLogistics
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ParcelDbContext>(options => options.UseInMemoryDatabase("ParcelDatabase"));

            //Register Services
            builder.Services.AddScoped<IParcelService, ParcelService>();

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            }); 

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();


            //Seed in memory
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ParcelDbContext>();

                db.Parcels.Add(new Parcel
                {
                    Barcode = "RMARS1234567890123456789M",
                    Sender = "Anders Hejlsberg",
                    Recipient = "Elon Musk",
                    DeliveryService = "Express",
                    Contents = "Signed C# language specification",
                    Status = ParcelStatus.Created,
                    Origin = "Starport Thames Estuary",
                    Destination = "New London",
                    LaunchDate = new DateTime(2025, 9, 3),
                    EtaDays = 90,
                    EstimatedArrivalDate = new DateTime(2025, 12, 2),
                    History = new List<ParcelHistory>
                    {
                        new ParcelHistory { Status = ParcelStatus.Created, Timestamp = DateTime.UtcNow }
                    }
                });

                db.SaveChanges();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            //Register Middleware Exception Handle class
            app.UseMiddleware<MiddleHandleException>();


            app.MapControllers();

            app.Run();
        }
    }
}
