using ASC.Model.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASC_Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public virtual DbSet<MasterDataKey> MasterDataKeys { get; set; }
        public virtual DbSet<MasterDataValue> MasterDataValues { get; set; }
        public virtual DbSet<ServiceRequest> ServiceRequests { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MasterDataKey>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(mdk => mdk.MasterDataValues)
                      .WithOne(mdv => mdv.MasterDataKey)
                      .HasForeignKey(mdv => mdv.MasterDataKeyId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<MasterDataValue>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            builder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(r => r.PricePerNight).HasColumnType("decimal(18,2)");
                entity.Property(r => r.CheckInDate).IsRequired(false);
                entity.Property(r => r.CheckOutDate).IsRequired(false);
                entity.Property(r => r.TotalPrice).HasColumnType("decimal(18,2)").IsRequired(false);

                entity.HasOne(r => r.RoomType)
                      .WithMany()
                      .HasForeignKey(r => r.RoomTypeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(b => b.CustomerName)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(b => b.Status)
                      .HasMaxLength(50);

                entity.Property(b => b.CheckInDate).IsRequired(false); // Cho phép null
                entity.Property(b => b.CheckOutDate).IsRequired(false); // Cho phép null


                entity.HasOne(b => b.Room)
                      .WithMany()
                      .HasForeignKey(b => b.RoomId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.User) // User là IdentityUser hoặc ApplicationUser
                      .WithMany() // Giả sử User không có collection Bookings
                      .HasForeignKey(b => b.UserId)
                      .IsRequired(false) // Cho phép UserId null
                      .OnDelete(DeleteBehavior.SetNull); // Nếu User bị xóa, UserId trong Booking thành null
            });

            builder.Entity<ServiceRequest>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
