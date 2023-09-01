

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AFSTranslate.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<TranslationType> TranslationTypes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=AFSDB;Integrated Security=True;TrustServerCertificate=true");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Response>()
           .HasOne(r => r.Status)
           .WithMany(s => s.Responses)
           .HasForeignKey(r => r.StatusId);

            modelBuilder.Entity<Response>()
                .HasOne(r => r.User)
                .WithMany(u => u.Responses)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Response>()
                .HasOne(r => r.Translation)
                .WithMany(t => t.Responses)
                .HasForeignKey(r => r.TranslationId);
        }


    }
}
