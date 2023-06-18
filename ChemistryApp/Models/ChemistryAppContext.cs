using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ChemistryApp.Models;

public partial class ChemistryAppContext : DbContext
{
    public ChemistryAppContext()
    {
    }

    public ChemistryAppContext(DbContextOptions<ChemistryAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CompletedChapter> CompletedChapters { get; set; }

    public virtual DbSet<CompletedTask> CompletedTasks { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCompletedChapter> UserCompletedChapters { get; set; }

    public virtual DbSet<UserCompletedTask> UserCompletedTasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=ChemistryApp;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompletedChapter>(entity =>
        {
            entity.ToTable("CompletedChapter");
        });

        modelBuilder.Entity<CompletedTask>(entity =>
        {
            entity.ToTable("CompletedTask");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasOne(d => d.FriendNavigation).WithMany(p => p.FriendFriendNavigations)
                .HasForeignKey(d => d.FriendId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Friends_User1");

            entity.HasOne(d => d.User).WithMany(p => p.FriendUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Friends_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Login).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(40);
        });

        modelBuilder.Entity<UserCompletedChapter>(entity =>
        {
            entity.ToTable("UserCompletedChapter");

            entity.HasOne(d => d.CompletedChapter).WithMany(p => p.UserCompletedChapters)
                .HasForeignKey(d => d.CompletedChapterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserCompletedChapter_CompletedChapter");

            entity.HasOne(d => d.User).WithMany(p => p.UserCompletedChapters)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserCompletedChapter_User");
        });

        modelBuilder.Entity<UserCompletedTask>(entity =>
        {
            entity.ToTable("UserCompletedTask");

            entity.HasOne(d => d.CompletedTask).WithMany(p => p.UserCompletedTasks)
                .HasForeignKey(d => d.CompletedTaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserCompletedTask_CompletedTask");

            entity.HasOne(d => d.User).WithMany(p => p.UserCompletedTasks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserCompletedTask_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
