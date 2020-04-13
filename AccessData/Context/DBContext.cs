using AccessData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccessData.Context
{
    public class DBContext : DbContext
    {
        public IConfiguration Configuration { get; }

        public DbSet<Move> Moves { get; set; }
        public DbSet<MoveDetail> MoveDetails { get; set; }

        public DBContext(DbContextOptions<DBContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Move>()
            .HasMany(b => b.MoveDetail)
            .WithOne();

            modelBuilder.Entity<MoveDetail>()
            .HasOne(p => p.Move)
            .WithMany(b => b.MoveDetail)
            .HasForeignKey(p => p.IdMove);
        }
    }
}
