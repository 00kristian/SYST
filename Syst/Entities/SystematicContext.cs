using System.Buffers;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Syst{

public class SystematicContext : DbContext, ISystematicContext
{
    public DbSet<Admin> admins { get; set; } = null!;
    public DbSet<Candidate> candidates {get;set;} = null!;
    public DbSet<Event> events {get;set;} = null!;
    public DbSet<Question> questions {get;set;} = null!;
    public DbSet<Quiz> quizes {get;set;} = null!;
    public string DbPath { get; private set; } = null!;

    public SystematicContext(DbContextOptions<SystematicContext> options): base(options) { }

    protected override async void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Candidate>()
        .HasMany<Event>(c => c.Events)
        .WithMany(e => e.Candidates);

        modelBuilder.Entity<Event>()
        .HasOne<Quiz>(e => e.Quiz)
        .WithMany(q => q.Events);
        
        modelBuilder.Entity<Question>()
        .HasOne<Quiz>(que => que.Quiz)
        .WithMany(q => q.Questions);

        modelBuilder.Entity<Quiz>()
        .HasMany<Candidate>(q => q.Candidates)
        .WithOne(c => c.Quiz);

        modelBuilder.Entity<Admin>()
        .HasMany<Event>(a => a.Events)
        .WithMany(e => e.Admins);

        modelBuilder.Entity<Question>()
        .Property(q => q.Options);

        modelBuilder
        .Entity<Candidate>()
        .Property(s => s.University)
        .HasMaxLength(50)
        .HasConversion(
            v => v.ToString(),
            v => (UniversityEnum)Enum.Parse(typeof(UniversityEnum), v));
    }
}
}