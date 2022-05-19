using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public interface ISystematicContext : IDisposable
{
    public DbSet<Candidate> Candidates {get;set;}
    public DbSet<Event> Events {get;set;}
    public DbSet<Question> Questions {get;set;}
    public DbSet<Quiz> Quizes {get;set;}
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}