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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}}