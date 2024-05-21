using System;
using System.Collections.Generic;
using APINTTShop.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace APINTTShop.Models;

public partial class NttshopContext : DbContext
{
    public NttshopContext()
    {
    }

    public NttshopContext(DbContextOptions<NttshopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Gestionusuario> Gestionusuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=NTTD-7R7F1F3\\SQLEXPRESS;user=lmaciabo;password=1234;database=nttshop; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Gestionusuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("GESTIONUSUARIO");

            entity.HasIndex(e => e.Inicio, "UQ__GESTIONU__704B4B35108F5202").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__GESTIONU__AB6E6164D2FCA75D").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Apellido1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellido1");
            entity.Property(e => e.Apellido2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellido2");
            entity.Property(e => e.Contrasenya)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contrasenya");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Inicio)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("inicio");
            entity.Property(e => e.IsoIdioma)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("isoIdioma");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("USUARIO");

            entity.HasIndex(e => e.Inicio, "UQ__USUARIO__704B4B356E05B769").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__USUARIO__AB6E616403A692BB").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Apellido1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellido1");
            entity.Property(e => e.Apellido2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellido2");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ciudad");
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("codigoPostal");
            entity.Property(e => e.Contrasenya)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contrasenya");
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IdRate).HasColumnName("idRate");
            entity.Property(e => e.Inicio)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("inicio");
            entity.Property(e => e.IsoIdioma)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("isoIdioma");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Provincia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("provincia");
            entity.Property(e => e.Telefono)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
