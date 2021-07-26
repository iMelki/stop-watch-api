using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Run> Runs { get; set; }
        public DbSet<Lap> Laps { get; set; }

        protected override void OnModelCreating(ModelBuilder builder){
            base.OnModelCreating(builder);

            builder.Entity<Lap>()
                .HasOne(lap => lap.Run)
                .WithMany(run => run.LapTimes)
                .HasForeignKey(lap => lap.RunId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}