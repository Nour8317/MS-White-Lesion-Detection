using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MS_white_lesion_detection.Models;
namespace MS_white_lesion_detection.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ScanSession> ScanSessions { get; set; }
        public DbSet<Scan> Scans { get; set; }
        public DbSet<Prediction> Predictions { get; set; }
        public DbSet<PredictionMask> PredictionMasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Prediction>()
                .HasOne(p => p.ScanSession)
                .WithOne(s => s.Prediction)
                .HasForeignKey<Prediction>(p => p.SessionId);
        }
    }
}
