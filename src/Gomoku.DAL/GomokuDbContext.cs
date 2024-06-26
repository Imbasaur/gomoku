﻿using Gomoku.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gomoku.DAL;
public class GomokuDbContext : DbContext
{
    public GomokuDbContext()
    { }

    public GomokuDbContext(DbContextOptions<GomokuDbContext> options) : base(options)
    { }

    public DbSet<Game> Games { get; set; }
    public DbSet<PlayerWaiting> WaitingList { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if DEBUG
        optionsBuilder.EnableSensitiveDataLogging(true);
#endif
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=127.0.0.1;Database=postgres;Username=postgres;Password=1d#f%gdsVC45#!!");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PlayerWaiting>()
            .HasIndex(pw => pw.PlayerName)
            .IsUnique();

        modelBuilder.Entity<Game>()
            .Property(g => g.Version)
            .IsRowVersion();
    }
}
