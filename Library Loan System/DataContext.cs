using Library_Loan_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Loan_System;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Loan> Loans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Student Configuration
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasIndex(s => s.Nim)
                  .IsUnique();
        });

        // 2. Loan Configuration
        modelBuilder.Entity<Loan>(entity =>
        {
            entity.Property(l => l.Fine)
                  .HasDefaultValue(0);

            entity.Property(l => l.Status)
                  .HasDefaultValue("Borrowed");

            // Relationships
            entity.HasOne(l => l.Student)
                  .WithMany(s => s.Loans)
                  .HasForeignKey(l => l.StudentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(l => l.Book)
                  .WithMany(b => b.Loans)
                  .HasForeignKey(l => l.BookId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}