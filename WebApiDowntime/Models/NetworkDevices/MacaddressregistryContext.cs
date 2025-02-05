using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace WebApiDowntime.Models.NetworkDevices;

public partial class MacaddressregistryContext : DbContext
{
    public MacaddressregistryContext()
    {
    }

    public MacaddressregistryContext(DbContextOptions<MacaddressregistryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Macaddresstable> Macaddresstables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=192.168.100.100;database=macaddressregistry;user=D_user;password=Aeroblock12345%;charset=utf8", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.22-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Macaddresstable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("macaddresstable")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.HasIndex(e => e.MacAdres, "MacAdres_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Id, "id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comment).HasMaxLength(120);
            entity.Property(e => e.IpAdres).HasMaxLength(15);
            entity.Property(e => e.MacAdres).HasMaxLength(17);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
