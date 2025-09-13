using Microsoft.EntityFrameworkCore;
using exampledotnet_project.Models;

namespace exampledotnet_project.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // Configure Product entity
        //    modelBuilder.Entity<Product>(entity =>
        //    {
        //        entity.HasKey(e => e.Id);
                
        //        entity.Property(e => e.Name)
        //            .IsRequired()
        //            .HasMaxLength(100);
                
        //        entity.Property(e => e.Description)
        //            .HasMaxLength(500);
                
        //        entity.Property(e => e.Price)
        //            .IsRequired()
        //            .HasPrecision(18, 2);
                
        //        entity.Property(e => e.Stock)
        //            .IsRequired();
                
        //        entity.Property(e => e.Category)
        //            .HasMaxLength(50);
                
        //        entity.Property(e => e.CreatedAt)
        //            .IsRequired()
        //            .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
        //        entity.Property(e => e.UpdatedAt)
        //            .IsRequired()
        //            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        //        // Create index on name for better search performance
        //        entity.HasIndex(e => e.Name);
                
        //        // Create index on category for filtering
        //        entity.HasIndex(e => e.Category);
        //    });
        //}

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Automatically update UpdatedAt timestamp
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Product && (e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                ((Product)entry.Entity).UpdatedAt = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}