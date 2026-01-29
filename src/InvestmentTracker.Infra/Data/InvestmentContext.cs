using Microsoft.EntityFrameworkCore;
using InvestmentTracker.Domain.Entities; // Make sure to import your Domain entities

namespace InvestmentTracker.Infra.Data
{
    public class InvestmentContext : DbContext
    {
        // 1. Define the Tables
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Snapshot> Snapshots { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<AssetTag> AssetTags { get; set; }

        // Constructor to allow configuration injection (e.g., from appsettings.json in the UI)
        public InvestmentContext(DbContextOptions<InvestmentContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // 1. Get the standard "LocalApplicationData" folder for the OS
                var folder = Environment.SpecialFolder.LocalApplicationData;
                var path = Environment.GetFolderPath(folder);

                // 2. Create a subfolder for your app
                var dbPath = System.IO.Path.Join(path, "InvestmentTracker");
                System.IO.Directory.CreateDirectory(dbPath); // Ensure folder exists

                // 3. Define the full file path
                var dbFile = System.IO.Path.Join(dbPath, "investments.db");

                // 4. Use it
                optionsBuilder.UseSqlite($"Data Source={dbFile}");
                
                // Optional: Print to console so you know where it is during dev
                Console.WriteLine($"Database Path: {dbFile}"); 
            }
        }

        // 2. Configure the Schema
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Asset Configuration ---
            modelBuilder.Entity<Asset>(entity =>
            {
                // Store the Enum as a String in the database (Readability)
                entity.Property(e => e.AssetType)
                      .HasConversion<string>();

                // Configure Fee precision (5 digits total, 4 decimal places -> 0.0050)
                entity.Property(e => e.FeePercentagePerYear)
                      .HasPrecision(5, 4) 
                      .HasDefaultValue(0.0000m);
                
                // Configure Delete Behavior: Deleting an Asset deletes its Snapshots & Contributions
                entity.HasMany(a => a.Snapshots)
                      .WithOne(s => s.Asset)
                      .HasForeignKey(s => s.AssetId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(a => a.Contributions)
                      .WithOne(c => c.Asset)
                      .HasForeignKey(c => c.AssetId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- Many-to-Many Relationship (Asset <-> Tag) ---
            modelBuilder.Entity<AssetTag>(entity =>
            {
                // Composite Primary Key
                entity.HasKey(at => new { at.AssetId, at.TagId });

                entity.HasOne(at => at.Asset)
                      .WithMany(a => a.AssetTags)
                      .HasForeignKey(at => at.AssetId);

                entity.HasOne(at => at.Tag)
                      .WithMany(t => t.AssetTags)
                      .HasForeignKey(at => at.TagId);
            });
            
            // --- Tag Configuration ---
            modelBuilder.Entity<Tag>(entity => 
            {
                entity.HasIndex(t => t.Name).IsUnique(); // Ensure Tag names are unique
            });
        }
    }
}