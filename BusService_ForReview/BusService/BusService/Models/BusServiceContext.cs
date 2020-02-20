using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BusService.Models
{
    public partial class BusServiceContext : DbContext
    {
        public BusServiceContext()
        {
        }

        public BusServiceContext(DbContextOptions<BusServiceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bus> Bus { get; set; }
        public virtual DbSet<BusRoute> BusRoute { get; set; }
        public virtual DbSet<BusStop> BusStop { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Driver> Driver { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<RouteSchedule> RouteSchedule { get; set; }
        public virtual DbSet<RouteStop> RouteStop { get; set; }
        public virtual DbSet<Trip> Trip { get; set; }
        public virtual DbSet<TripStop> TripStop { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\;Database=BusService;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Bus>(entity =>
            {
                entity.ToTable("bus");

                entity.Property(e => e.BusId).HasColumnName("busId");

                entity.Property(e => e.BusNumber).HasColumnName("busNumber");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BusRoute>(entity =>
            {
                entity.HasKey(e => e.BusRouteCode);

                entity.ToTable("busRoute");

                entity.Property(e => e.BusRouteCode)
                    .HasColumnName("busRouteCode")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.RouteName)
                    .IsRequired()
                    .HasColumnName("routeName")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<BusStop>(entity =>
            {
                entity.HasKey(e => e.BusStopNumber);

                entity.ToTable("busStop");

                entity.Property(e => e.BusStopNumber)
                    .HasColumnName("busStopNumber")
                    .ValueGeneratedNever();

                entity.Property(e => e.GoingDowntown).HasColumnName("goingDowntown");

                entity.Property(e => e.Location)
                    .HasColumnName("location")
                    .HasMaxLength(50);

                entity.Property(e => e.LocationHash).HasColumnName("locationHash");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryCode);

                entity.ToTable("country");

                entity.Property(e => e.CountryCode)
                    .HasColumnName("countryCode")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhonePattern)
                    .HasColumnName("phonePattern")
                    .IsUnicode(false);

                entity.Property(e => e.PostalPattern)
                    .HasColumnName("postalPattern")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.ToTable("driver");

                entity.Property(e => e.DriverId).HasColumnName("driverId");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateHired)
                    .HasColumnName("dateHired")
                    .HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .HasColumnName("firstName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasColumnName("fullName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HomePhone)
                    .HasColumnName("homePhone")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasColumnName("lastName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PostalCode)
                    .HasColumnName("postalCode")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ProvinceCode)
                    .HasColumnName("provinceCode")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .HasColumnName("street")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WorkPhone)
                    .HasColumnName("workPhone")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProvinceCodeNavigation)
                    .WithMany(p => p.Driver)
                    .HasForeignKey(d => d.ProvinceCode)
                    .HasConstraintName("FK_driver_province");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.HasKey(e => e.ProvinceCode);

                entity.ToTable("province");

                entity.Property(e => e.ProvinceCode)
                    .HasColumnName("provinceCode")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Capital)
                    .HasColumnName("capital")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasColumnName("countryCode")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TaxCode)
                    .HasColumnName("taxCode")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TaxRate).HasColumnName("taxRate");

                entity.HasOne(d => d.CountryCodeNavigation)
                    .WithMany(p => p.Province)
                    .HasForeignKey(d => d.CountryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_province_country");
            });

            modelBuilder.Entity<RouteSchedule>(entity =>
            {
                entity.ToTable("routeSchedule");

                entity.Property(e => e.RouteScheduleId).HasColumnName("routeScheduleId");

                entity.Property(e => e.BusRouteCode)
                    .HasColumnName("busRouteCode")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((2))");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .IsUnicode(false);

                entity.Property(e => e.IsWeekDay).HasColumnName("isWeekDay");

                entity.Property(e => e.StartTime).HasColumnName("startTime");

                entity.HasOne(d => d.BusRouteCodeNavigation)
                    .WithMany(p => p.RouteSchedule)
                    .HasForeignKey(d => d.BusRouteCode)
                    .HasConstraintName("FK_routeSchedule_busRoute");
            });

            modelBuilder.Entity<RouteStop>(entity =>
            {
                entity.ToTable("routeStop");

                entity.Property(e => e.RouteStopId).HasColumnName("routeStopId");

                entity.Property(e => e.BusRouteCode)
                    .HasColumnName("busRouteCode")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((2))");

                entity.Property(e => e.BusStopNumber).HasColumnName("busStopNumber");

                entity.Property(e => e.OffsetMinutes).HasColumnName("offsetMinutes");

                entity.HasOne(d => d.BusRouteCodeNavigation)
                    .WithMany(p => p.RouteStop)
                    .HasForeignKey(d => d.BusRouteCode)
                    .HasConstraintName("FK_routeStop_busRoute");

                entity.HasOne(d => d.BusStopNumberNavigation)
                    .WithMany(p => p.RouteStop)
                    .HasForeignKey(d => d.BusStopNumber)
                    .HasConstraintName("FK_routeStop_busStop");
            });

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.ToTable("trip");

                entity.Property(e => e.TripId).HasColumnName("tripId");

                entity.Property(e => e.BusId).HasColumnName("busId");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .IsUnicode(false);

                entity.Property(e => e.DriverId).HasColumnName("driverId");

                entity.Property(e => e.RouteScheduleId).HasColumnName("routeScheduleId");

                entity.Property(e => e.TripDate)
                    .HasColumnName("tripDate")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Bus)
                    .WithMany(p => p.Trip)
                    .HasForeignKey(d => d.BusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trip_bus");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.Trip)
                    .HasForeignKey(d => d.DriverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trip_driver");

                entity.HasOne(d => d.RouteSchedule)
                    .WithMany(p => p.Trip)
                    .HasForeignKey(d => d.RouteScheduleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trip_routeSchedule");
            });

            modelBuilder.Entity<TripStop>(entity =>
            {
                entity.ToTable("tripStop");

                entity.Property(e => e.TripStopId).HasColumnName("tripStopId");

                entity.Property(e => e.BusStopNumber).HasColumnName("busStopNumber");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .IsUnicode(false);

                entity.Property(e => e.TripId).HasColumnName("tripId");

                entity.Property(e => e.TripStopTime).HasColumnName("tripStopTime");

                entity.HasOne(d => d.BusStopNumberNavigation)
                    .WithMany(p => p.TripStop)
                    .HasForeignKey(d => d.BusStopNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tripStop_busStop");

                entity.HasOne(d => d.Trip)
                    .WithMany(p => p.TripStop)
                    .HasForeignKey(d => d.TripId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tripStop_trip");
            });
        }
    }
}
