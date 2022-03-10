using System.Buffers;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure;

public class SystematicContext : DbContext, ISystematicContext
{
    public DbSet<Admin> Admins { get; set; } = null!;
    public DbSet<Candidate> Candidates {get;set;} = null!;
    public DbSet<Event> Events {get;set;} = null!;
    public DbSet<Question> Questions {get;set;} = null!;
    public DbSet<Quiz> Quizes {get;set;} = null!;
    public string DbPath { get; private set; } = null!;

    public SystematicContext(DbContextOptions<SystematicContext> options): base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Candidate>()
        .HasMany<Event>(c => c.EventsParticipatedIn)
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
        .HasMany<Event>(a => a.Events);

        modelBuilder.Entity<Question>()
        .Property(q => q.Options);

        modelBuilder.Entity<Candidate>()
        .Property(c => c.Answers);

        modelBuilder
        .Entity<Candidate>()
        .Property(s => s.University)
        .HasMaxLength(50)
        .HasConversion(
            v => v.ToString(),
            v => (UniversityEnum)Enum.Parse(typeof(UniversityEnum), v));

        modelBuilder.Entity<Candidate>()
        .Property(c => c.Answers)
        .HasConversion(
            v => JsonSerializer.Serialize(v!.Select(d => d.ToString()).ToList(), default(JsonSerializerOptions)),
            v => JsonSerializer.Deserialize<List<string>>(v, default(JsonSerializerOptions))!.Select(d => (bool)Enum.Parse(typeof(bool), d)).ToList(),
            new ValueComparer<ICollection<bool>>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList())
        );    

        modelBuilder.Entity<Question>()
        .Property(q => q.Options)
        .HasConversion(
            v => JsonSerializer.Serialize(v!.Select(d => d.ToString()).ToList(), default(JsonSerializerOptions)),
            v => JsonSerializer.Deserialize<List<string>>(v, default(JsonSerializerOptions))!.Select(d => (string)Enum.Parse(typeof(string), d)).ToList(),
            new ValueComparer<ICollection<string>>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList())
        );  
    }
}