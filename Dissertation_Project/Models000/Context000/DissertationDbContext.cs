using System;
using System.Collections.Generic;
using Dissertation_Project.Models000;
using Microsoft.EntityFrameworkCore;

namespace Dissertation_Project.Models000.Context000;

public partial class DissertationDbContext : DbContext
{
    public DissertationDbContext()
    {
    }

    public DissertationDbContext(DbContextOptions<DissertationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<Baslookup> Baslookups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ALIMOOSAEI\\SQL_SERVER2019; Initial Catalog=Dissertation_Db; TrustServerCertificate=True; MultiSubnetFailover=True; Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasOne(d => d.BasLookupRefNavigation).WithMany(p => p.AspNetUsers)
                .HasForeignKey(d => d.BasLookupRef)
                .HasConstraintName("FK_aspnetUsers_BasLookup");
        });

        modelBuilder.Entity<Baslookup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BASLooku__3214EC07A4E6B86E");

            entity.ToTable("BASLookup");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Title).HasMaxLength(500);
            entity.Property(e => e.Type).HasMaxLength(250);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
