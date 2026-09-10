using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using DiagnosisRepositoryApi.Entities;

namespace DiagnosisRepositoryApi.Data;

public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
{
    public DbSet<Record> Diagnoses { get; set; }
    public DbSet<Customer> Patients { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Customer>()
        .Property(x => x.Id)
        .HasConversion<string>();

    modelBuilder.Entity<Record>()
        .Property(x => x.SourceId)
        .HasConversion<string>();

    modelBuilder.Entity<Record>()
        .Property(x => x.PatientId)
        .HasConversion<string>();

    modelBuilder.Entity<Record>()
        .Property(x => x.VisitId)
        .HasConversion<string>();

    modelBuilder.Entity<Customer>()
        .HasMany(x => x.Diagnoses)
        .WithOne(x => x.Patient)
        .HasForeignKey(x => x.PatientId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Record>()
        .HasKey(x => new { x.Provider, x.SourceId });
}
}
