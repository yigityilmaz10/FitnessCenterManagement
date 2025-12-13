// Dosya: Controllers/Data/ApplicationDbContext.cs
using FitnessCenterManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterManagement.Data
{
    // IdentityDbContext, Kimlik/Kullanıcı yönetimi için gerekli tabloları otomatik ekler.
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tüm varlıklar (Entityler).
        public DbSet<Gym> Gyms { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Randevu -> Antrenör İlişkisi (DeleteBehavior.Restrict, veri bütünlüğü için kritik)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Trainer)
                .WithMany(t => t.Appointments)
                .HasForeignKey(a => a.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Randevu -> Hizmet İlişkisi
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany()
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Decimal Hassasiyeti
            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasPrecision(18, 2);
        }
    }
}