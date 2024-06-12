using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DocumentApprovalSystem.Data;

public partial class DocumentApprovalDbContext : DbContext
{
    public DocumentApprovalDbContext()
    {
    }

    public DocumentApprovalDbContext(DbContextOptions<DocumentApprovalDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DocRequest> DocRequests { get; set; }

    public virtual DbSet<DocType> DocTypes { get; set; }

    public virtual DbSet<EmailConfig> EmailConfigs { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployerPemSec> EmployerPemSecs { get; set; }

    public virtual DbSet<EmployerSec> EmployerSecs { get; set; }

    public virtual DbSet<Title> Titles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=DocumentApprovalDb;Username=postgres;Password=Gody");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.ToTable("Attachment");

            entity.HasIndex(e => e.CreatorId, "IX_Attachment_CreatorId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Container).HasMaxLength(64);
            entity.Property(e => e.Extension).HasMaxLength(8);
            entity.Property(e => e.Name).HasMaxLength(256);

            entity.HasOne(d => d.Creator).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Attachment_CreatorId_fkey");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Department_pkey");

            entity.ToTable("Department");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Name).HasColumnType("character varying");
        });

        modelBuilder.Entity<DocRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LeaveRequest_pkey");

            entity.ToTable("DocRequest");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AttachmentsJson).HasColumnType("character varying");
            entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.EndDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.NotesJson).HasColumnType("character varying");
            entity.Property(e => e.Number).HasColumnType("character varying");
            entity.Property(e => e.RequestComments).HasColumnType("character varying");
            entity.Property(e => e.StartDate).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Creator).WithMany(p => p.DocRequests)
                .HasForeignKey(d => d.CreatorId)
                .HasConstraintName("Creator");

            entity.HasOne(d => d.Department).WithMany(p => p.DocRequests)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("Department");

            entity.HasOne(d => d.DocType).WithMany(p => p.DocRequests)
                .HasForeignKey(d => d.DocTypeId)
                .HasConstraintName("DocType");

            entity.HasOne(d => d.Employee).WithMany(p => p.DocRequests)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("Employee");
        });

        modelBuilder.Entity<DocType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DocType_pkey");

            entity.ToTable("DocType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Name).HasColumnType("character varying");
        });

        modelBuilder.Entity<EmailConfig>(entity =>
        {
            entity.ToTable("EmailConfig");

            entity.HasIndex(e => e.CreatorId, "IX_EmailConfig_CreatorId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Hash).HasMaxLength(256);
            entity.Property(e => e.Host).HasMaxLength(64);
            entity.Property(e => e.Name).HasMaxLength(64);
            entity.Property(e => e.SenderDisplayName).HasMaxLength(128);
            entity.Property(e => e.SenderId).HasMaxLength(128);
            entity.Property(e => e.Username).HasMaxLength(128);

            entity.HasOne(d => d.Creator).WithMany(p => p.EmailConfigs)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_EmailConfig_User");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Employee_pkey");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.ProfilePictureId, "IX_Employee_ProfilePictureId");

            entity.HasIndex(e => e.TitleId, "IX_Employee_TitleId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("Id ");
            entity.Property(e => e.AccountId).HasMaxLength(32);
            entity.Property(e => e.Address).HasMaxLength(256);
            entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.DoB).HasColumnType("timestamp without time zone");
            entity.Property(e => e.EcNumber).HasMaxLength(64);
            entity.Property(e => e.FirstName).HasMaxLength(256);
            entity.Property(e => e.IdNumber).HasMaxLength(32);
            entity.Property(e => e.Position)
                .HasMaxLength(128)
                .HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Surname).HasMaxLength(128);

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("Employee_Department");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Employee)
                .HasForeignKey<Employee>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Employee_User");

            entity.HasOne(d => d.ProfilePicture).WithMany(p => p.Employees)
                .HasForeignKey(d => d.ProfilePictureId)
                .HasConstraintName("Employee_ProfilePictureId_fkey");

            entity.HasOne(d => d.Title).WithMany(p => p.Employees)
                .HasForeignKey(d => d.TitleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Employee_TitleId_fkey");
        });

        modelBuilder.Entity<EmployerPemSec>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EmployerPemSec_pkey");

            entity.ToTable("EmployerPemSec");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Email).HasColumnType("character varying");
            entity.Property(e => e.Name).HasColumnType("character varying");
            entity.Property(e => e.Phone).HasColumnType("character varying");
        });

        modelBuilder.Entity<EmployerSec>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EmployerSec_pkey");

            entity.ToTable("EmployerSec");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasColumnType("character varying");
            entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Email).HasColumnType("character varying");
            entity.Property(e => e.Phone).HasColumnType("character varying");
        });

        modelBuilder.Entity<Title>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Title_pkey");

            entity.ToTable("Title");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Name).HasColumnType("character varying");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasIndex(e => e.CreatorId, "IX_User_CreatorId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ActivationDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.AuthRecoveryCodes).HasMaxLength(512);
            entity.Property(e => e.AuthenticatorKey).HasMaxLength(128);
            entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Email).HasMaxLength(128);
            entity.Property(e => e.LastLoginDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.LockoutExpiryDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.LoginId).HasMaxLength(128);
            entity.Property(e => e.Mobile).HasMaxLength(16);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.SecurityStamp).HasMaxLength(256);

            entity.HasOne(d => d.Creator).WithMany(p => p.InverseCreator)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("User_CreatorId_fkey");

            entity.HasOne(d => d.EmployerPemSec).WithMany(p => p.Users)
                .HasForeignKey(d => d.EmployerPemSecId)
                .HasConstraintName("User_EmployerPemSecId_fkey");

            entity.HasOne(d => d.EmployerSec).WithMany(p => p.Users)
                .HasForeignKey(d => d.EmployerSecId)
                .HasConstraintName("User_EmployerSecId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
