using Microsoft.EntityFrameworkCore;
using ScryfallAPI.Models;

namespace ScryfallAPI
{
    public class ScryfallContext : DbContext
    {
        public ScryfallContext(DbContextOptions options) : base(options) { }

        public ScryfallContext() { }

        public DbSet<FavoriteCards> Favorites { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
         => modelBuilder.Entity<FavoriteCards>(entity =>
         {
             entity.ToTable("Favorites");

             entity.HasKey(e => e.Id);

             entity.HasOne(e => e.User)
                   .WithMany(e => e.Favorites);

             entity.Property(e => e.Id).HasColumnName("Id");

             entity.Property(e => e.Name).HasColumnName("Name");
             
             entity.Property(e => e.IsFavorite).HasColumnName("IsFavorite");
             
             entity.Property(e => e.ReleasedAt).HasColumnName("ReleaseDate");
             
             entity.Property(e => e.PennyRank).HasColumnName("PennyRank");

             entity.Property(e => e.UserId).HasColumnName("UserId");
         }).Entity<User>(user => {

             user.ToTable("Users");

             user.HasKey(e => e.Id);

             user.Property(e => e.Id).HasColumnName("Id");

             user.Property(e => e.Email).HasColumnName("Email");
         });
    }
}
