using System.Threading;
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Core;

namespace Server.Model;

public static class SeedExtensions
{
    public static async Task<IHost> SeedAsync(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<SystematicContext>();

            await SeedProjectsAsync(context);
        }
        return host;
    }

    private static async Task SeedProjectsAsync(SystematicContext context)
    {
        if (context.Admins.Count() > 1) return;

        await context.Database.MigrateAsync();

        if (!await context.Admins.AnyAsync())
        {

            context.Admins.AddRange(
                new Admin() {Name = "Bob Bobsen", Email = "bob@bobsen.com", Events = new List<Event> {}}
            );

            await context.SaveChangesAsync();
        }
    }
}