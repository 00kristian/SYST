
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure;

public class SystematicContext : DbContext, ISystematicContext
{
    //Tables in our database 
    public DbSet<Admin> Admins { get; set; } = null!;
    public DbSet<Candidate> Candidates { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Quiz> Quizes { get; set; } = null!;
    public DbSet<Answer> Answers { get; set; } = null!;
    public string DbPath { get; private set; } = null!;

    public SystematicContext(DbContextOptions<SystematicContext> options) : base(options) { }

    
    //Creates our relationships
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        //Many to many relationship between candidate and event
        modelBuilder.Entity<Candidate>()
        .HasMany<Event>(c => c.EventsParticipatedIn)
        .WithMany(e => e.Candidates);

        //One to many relationship between event and quiz
        modelBuilder.Entity<Event>()
        .HasOne<Quiz>(e => e.Quiz)
        .WithMany(q => q.Events);

        //One to many relationship between question and quiz
        modelBuilder.Entity<Question>()
        .HasOne<Quiz>(que => que.Quiz)
        .WithMany(q => q.Questions);

        //One to many relationship between quiz and candidate
        modelBuilder.Entity<Quiz>()
        .HasMany<Candidate>(q => q.Candidates)
        .WithOne(c => c.Quiz);

        //To make sure admins know what events they have rated 
        modelBuilder.Entity<Admin>()
        .HasMany<Event>(a => a.Events);

        //Makes sure we can store a list of answers in candidate
        modelBuilder.Entity<Answer>()
        .Property(a => a.Answers)
        .HasConversion(
            v => JsonSerializer.Serialize(v!.Select(d => d.ToString()).ToList(), default(JsonSerializerOptions)),
            v => JsonSerializer.Deserialize<List<string>>(v, default(JsonSerializerOptions))!.ToArray(),
            new ValueComparer<string[]>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToArray())
        );

        //makes sure we can store a list of options in question
        modelBuilder.Entity<Question>()
        .Property(q => q.Options)
        .HasConversion(
            v => JsonSerializer.Serialize(v!.Select(d => d.ToString()).ToList(), default(JsonSerializerOptions)),
            v => JsonSerializer.Deserialize<List<string>>(v, default(JsonSerializerOptions))!.ToList(),
            new ValueComparer<ICollection<string>>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList())
        );

        modelBuilder.Entity<Event>().HasMany<Candidate>(e => e.Winners);
    }
}