using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DL;

public partial class IespinozaProgramacionNcapasGf2023Context : DbContext
{
    public IespinozaProgramacionNcapasGf2023Context()
    {
    }

    public IespinozaProgramacionNcapasGf2023Context(DbContextOptions<IespinozaProgramacionNcapasGf2023Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Alumno> Alumnos { get; set; }

    public virtual DbSet<Grupo> Grupos { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<Plantel> Plantels { get; set; }

    public virtual DbSet<Semestre> Semestres { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.; Database= IEspinozaProgramacionNCapasGF2023; User ID=sa; TrustServerCertificate=True; Password=pass@word1;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alumno>(entity =>
        {
            entity.HasKey(e => e.IdAlumno).HasName("PK__Alumno__460B47405EED8BD8");

            entity.ToTable("Alumno");

            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaNacimiento).HasColumnType("date");
            entity.Property(e => e.Imagen).IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdSemestreNavigation).WithMany(p => p.Alumnos)
                .HasForeignKey(d => d.IdSemestre)
                .HasConstraintName("FK__Alumno__IdSemest__36B12243");
        });

        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.HasKey(e => e.IdGrupo).HasName("PK__Grupo__303F6FD99C894A16");

            entity.ToTable("Grupo");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdPlantelNavigation).WithMany(p => p.Grupos)
                .HasForeignKey(d => d.IdPlantel)
                .HasConstraintName("FK__Grupo__IdPlantel__6D0D32F4");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario).HasName("PK__Horario__1539229B15F57257");

            entity.ToTable("Horario");

            entity.Property(e => e.Turno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("turno");

            entity.HasOne(d => d.IdAlumnoNavigation).WithMany(p => p.Horarios)
                .HasForeignKey(d => d.IdAlumno)
                .HasConstraintName("FK__Horario__IdAlumn__6477ECF3");
        });

        modelBuilder.Entity<Plantel>(entity =>
        {
            entity.HasKey(e => e.IdPlantel).HasName("PK__Plantel__485FDCFE6F3ABB65");

            entity.ToTable("Plantel");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Semestre>(entity =>
        {
            entity.HasKey(e => e.IdSemestre).HasName("PK__Semestre__BD1FD7F8E78234D6");

            entity.ToTable("Semestre");

            entity.Property(e => e.IdSemestre).ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
