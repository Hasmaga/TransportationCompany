using Microsoft.EntityFrameworkCore;
using TransportationCompany.Model;

namespace TransportationCompany.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        // relatetionship 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RouteTrip>()
                .HasOne(l => l.From)
                .WithMany(l => l.FromLocation)
                .HasForeignKey(l => l.FromLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<RouteTrip>()
                .HasOne(l => l.To)
                .WithMany(l => l.ToLocation)
                .HasForeignKey(l => l.ToLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
        

        // Create the DbSet properties for the entities

        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<RouteTrip> RouteTrips { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<PassengerComment> PassengerComments { get; set; }
        public DbSet<PassengerLogin> PassengerLogins { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
    }
}
