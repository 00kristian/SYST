using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public interface ISystematicContext : IDisposable
{
    public DbSet<Admin> admins { get; set; }
    public DbSet<Candidate> candidates {get;set;}
    public DbSet<Event> events {get;set;}
    public DbSet<Question> questions {get;set;}
    public DbSet<Quiz> quizes {get;set;}
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}