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

            context.Candidates.AddRange(
                new Candidate() {Name = "Lukas Hjelmstrand", Email = "luhj@itu.dk", StudyProgram = "Bsc i Softwareudvikling", University = UniversityEnum.ITU},
                new Candidate() {Name = "Rene Dif", Email = "rene@dif.dk", StudyProgram = "Msc i Vand", University = UniversityEnum.CBS},
                new Candidate() {Name = "Isabella Magnusdottir", Email = "isab3ll4@gmail.com", StudyProgram = "PHD i Sexologi", University = UniversityEnum.RUC},
                new Candidate() {Name = "Camille Gonnsen", Email = "camg@itu.dk", StudyProgram = "Bsc i Rødhårethed", University = UniversityEnum.ITU},
                new Candidate() {Name = "Kristian Berlin Jensen", Email = "berlin@itu.dk", StudyProgram = "Bsc i Tysklandsstudier", University = UniversityEnum.KU}
            );

            await context.SaveChangesAsync();
        }
    }
}