using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeminarHub.Data.Models;
using System.Reflection.Emit;

namespace SeminarHub.Data
{
    public class SeminarHubDbContext : IdentityDbContext
    {
        public SeminarHubDbContext(DbContextOptions<SeminarHubDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SeminarParticipant>()
                .HasKey(ep => new { ep.SeminarId, ep.ParticipantId });

            builder.Entity<Seminar>()
             .HasMany(s => s.SeminarsParticipants)
             .WithOne(p => p.Seminar)
             .OnDelete(DeleteBehavior.NoAction);

            builder
               .Entity<Category>()
               .HasData(new Category()
               {
                   Id = 1,
                   Name = "Technology & Innovation"
               },
               new Category()
               {
                   Id = 2,
                   Name = "Business & Entrepreneurship"
               },
               new Category()
               {
                   Id = 3,
                   Name = "Science & Research"
               },
               new Category()
               {
                   Id = 4,
                   Name = "Arts & Culture"
               });

            base.OnModelCreating(builder);
        }
        public DbSet<SeminarParticipant> SeminarsParticipants { get; set; }
        public DbSet<Seminar> Seminars { get; set; }
        public DbSet<Category> Categorys { get; set; }
    }
}