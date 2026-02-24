using Microsoft.EntityFrameworkCore;
using GraduationProject.Entites;

namespace GraduationProject.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Entites.Route> Routes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<BusLocation> BusLocations { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<PasswordResetCode> PasswordResetCodes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ticket -> User
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ticket -> Route  ❗ مهم
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Route)
                .WithMany()
                .HasForeignKey(t => t.RouteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ticket -> Bus ❗ مهم
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Bus)
                .WithMany(b => b.Tickets)
                .HasForeignKey(t => t.BusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Station>()
                .HasOne(s => s.Route)
                .WithMany(r => r.Stations)
                .HasForeignKey(s => s.RouteId);

            modelBuilder.Entity<Bus>()
                .HasOne(b => b.Route)
                .WithMany(r => r.Buses)
                .HasForeignKey(b => b.RouteId);

            modelBuilder.Entity<Entites.Route>()
                .HasOne(r => r.Zone)
                .WithMany(z => z.Routes)
                .HasForeignKey(r => r.ZoneId);

            modelBuilder.Entity<BusLocation>()
                .HasOne(bl => bl.Bus)
                .WithMany(b => b.BusLocations)
                .HasForeignKey(bl => bl.BusId);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.User)
                .WithMany(u => u.Complaints)
                .HasForeignKey(c => c.UserId);
        }

    }
}
