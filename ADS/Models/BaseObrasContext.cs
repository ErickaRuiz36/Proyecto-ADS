using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ADS.Models;

public partial class BaseObrasContext : DbContext
{
    public BaseObrasContext()
    {
    }

    public BaseObrasContext(DbContextOptions<BaseObrasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActividadesReporte> ActividadesReportes { get; set; }

    public virtual DbSet<Concepto> Conceptos { get; set; }

    public virtual DbSet<ConceptosFactura> ConceptosFacturas { get; set; }

    public virtual DbSet<Constructora> Constructoras { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<EstadoProblema> EstadoProblemas { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<FrenteObra> FrenteObras { get; set; }

    public virtual DbSet<ImagenesReporte> ImagenesReportes { get; set; }

    public virtual DbSet<ProblemasReporte> ProblemasReportes { get; set; }

    public virtual DbSet<Proyecto> Proyectos { get; set; }

    public virtual DbSet<RegistroActividade> RegistroActividades { get; set; }

    public virtual DbSet<ReporteDiario> ReportesDiarios { get; set; }

    public virtual DbSet<TipoConcepto> TipoConceptos { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
/*    
 *    Sí es necesario comentar lo de arriba xdnt
 *    
 *    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:proyectoads.database.windows.net,1433;Database=BaseObras;User ID=erickaruiz36;Password=contra222#;TrustServerCertificate=True;");
*/    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActividadesReporte>(entity =>
        {
            entity.HasKey(e => e.IdActividadesReporte).HasName("PK__Activida__3C865228FEAD5968");

            entity.ToTable("ActividadesReporte");

            entity.Property(e => e.IdActividadesReporte).HasColumnName("idActividadesReporte");
            entity.Property(e => e.Actividad)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.IdReporte).HasColumnName("idReporte");

            entity.HasOne(d => d.IdReporteNavigation).WithMany(p => p.ActividadesReportes)
                .HasForeignKey(d => d.IdReporte)
                .HasConstraintName("FK__Actividad__idRep__76969D2E");
        });

        modelBuilder.Entity<Concepto>(entity =>
        {
            entity.HasKey(e => e.IdConcepto).HasName("PK__Concepto__25A881FD645D6114");

            entity.ToTable("Concepto");

            entity.Property(e => e.IdConcepto).HasColumnName("idConcepto");
            entity.Property(e => e.Cantidad).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Concepto1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Concepto");
            entity.Property(e => e.CostoUnidad).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Unidad)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.IdConceptoBase).HasColumnName("idConceptoBase");
            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.IdTipoConcepto).HasColumnName("idTipoConcepto");
            
            entity.Property(e => e.IdFrenteObra).HasColumnName("idFrenteObra");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Conceptos)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Concepto__idEsta__08B54D69");

            entity.HasOne(d => d.IdTipoConceptoNavigation).WithMany(p => p.Conceptos)
                .HasForeignKey(d => d.IdTipoConcepto)
                .HasConstraintName("FK__Concepto__idTipo__07C12930");

            entity.HasOne(d => d.IdFrenteObraNavigation).WithMany(p => p.Conceptos)
                .HasForeignKey(d => d.IdFrenteObra)
                .HasConstraintName("FK__Concepto__idFren__1F98B2C1");
        });

        modelBuilder.Entity<ConceptosFactura>(entity =>
        {
            entity.HasKey(e => e.IdConceptosFactura).HasName("PK__Concepto__0F718C530293C17F");

            entity.ToTable("ConceptosFactura");

            entity.Property(e => e.IdConceptosFactura).HasColumnName("idConceptosFactura");
            entity.Property(e => e.IdConcepto).HasColumnName("idConcepto");
            entity.Property(e => e.IdFactura).HasColumnName("idFactura");

            entity.HasOne(d => d.IdConceptoNavigation).WithMany(p => p.ConceptosFacturas)
                .HasForeignKey(d => d.IdConcepto)
                .HasConstraintName("FK__Conceptos__idCon__0C85DE4D");

           
        });

        modelBuilder.Entity<Constructora>(entity =>
        {
            entity.HasKey(e => e.IdConstructora).HasName("PK__Construc__26EDEB41A0E1EA2B");

            entity.ToTable("Constructora");

            entity.Property(e => e.IdConstructora).HasColumnName("idConstructora");
            entity.Property(e => e.CorreoContacto)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Rfc)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("RFC");
            entity.Property(e => e.Telefono)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__Estado__62EA894A59D8455A");

            entity.ToTable("Estado");

            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.Estado1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Estado");
        });

        modelBuilder.Entity<EstadoProblema>(entity =>
        {
            entity.HasKey(e => e.IdEstadoProblema).HasName("PK__EstadoPr__31103E08F491172E");

            entity.ToTable("EstadoProblema");

            entity.Property(e => e.IdEstadoProblema).HasColumnName("idEstadoProblema");
            entity.Property(e => e.Estado)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("PK__Factura__3CD5687E1668F080");

            entity.ToTable("Factura");

            entity.Property(e => e.IdFactura).HasColumnName("idFactura");
            entity.Property(e => e.DescripcionServicio)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.IdFrenteObra).HasColumnName("idFrenteObra");
            entity.Property(e => e.IdProyecto).HasColumnName("idProyecto");
            entity.Property(e => e.Periodo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SubTotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Factura__idEstad__02FC7413");

            entity.HasOne(d => d.IdFrenteObraNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdFrenteObra)
                .HasConstraintName("FK__Factura__idFrent__01142BA1");

            entity.HasOne(d => d.IdProyectoNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdProyecto)
                .HasConstraintName("FK__Factura__idProye__02084FDA");
        });

        modelBuilder.Entity<FrenteObra>(entity =>
        {
            entity.HasKey(e => e.IdFrenteObra).HasName("PK__FrenteOb__43A7FB93538E9D11");

            entity.ToTable("FrenteObra");

            entity.Property(e => e.IdFrenteObra).HasColumnName("idFrenteObra");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.IdConstructora).HasColumnName("idConstructora");
            entity.Property(e => e.IdProyecto).HasColumnName("idProyecto");
            entity.Property(e => e.IdResidente).HasColumnName("idResidente");
            entity.Property(e => e.IdSuperintendente).HasColumnName("idSuperintendente");
            entity.Property(e => e.IdSupervisor).HasColumnName("idSupervisor");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdConstructoraNavigation).WithMany(p => p.FrenteObras)
                .HasForeignKey(d => d.IdConstructora)
                .HasConstraintName("FK__FrenteObr__idCon__693CA210");

            entity.HasOne(d => d.IdProyectoNavigation).WithMany(p => p.FrenteObras)
                .HasForeignKey(d => d.IdProyecto)
                .HasConstraintName("FK__FrenteObr__idPro__656C112C");

            entity.HasOne(d => d.IdResidenteNavigation).WithMany(p => p.FrenteObraIdResidenteNavigations)
                .HasForeignKey(d => d.IdResidente)
                .HasConstraintName("FK__FrenteObr__idRes__66603565");

            entity.HasOne(d => d.IdSuperintendenteNavigation).WithMany(p => p.FrenteObraIdSuperintendenteNavigations)
                .HasForeignKey(d => d.IdSuperintendente)
                .HasConstraintName("FK__FrenteObr__idSup__68487DD7");

            entity.HasOne(d => d.IdSupervisorNavigation).WithMany(p => p.FrenteObraIdSupervisorNavigations)
                .HasForeignKey(d => d.IdSupervisor)
                .HasConstraintName("FK__FrenteObr__idSup__6754599E");
        });

        modelBuilder.Entity<ImagenesReporte>(entity =>
        {
            entity.HasKey(e => e.IdImagenesReporte).HasName("PK__Imagenes__AC666A42C0A37A5A");

            entity.ToTable("ImagenesReporte");

            entity.Property(e => e.IdImagenesReporte).HasColumnName("idImagenesReporte");
            entity.Property(e => e.IdReporte).HasColumnName("idReporte");
            entity.Property(e => e.Src)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdReporteNavigation).WithMany(p => p.ImagenesReportes)
                .HasForeignKey(d => d.IdReporte)
                .HasConstraintName("FK__ImagenesR__idRep__7E37BEF6");
        });

        modelBuilder.Entity<ProblemasReporte>(entity =>
        {
            entity.HasKey(e => e.IdProblemasReporte).HasName("PK__Problema__A48297C413BC61A8");

            entity.ToTable("ProblemasReporte");

            entity.Property(e => e.IdProblemasReporte).HasColumnName("idProblemasReporte");
            entity.Property(e => e.ActividadesCorrectivas)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.IdEstadoProblema).HasColumnName("idEstadoProblema");
            entity.Property(e => e.Problema)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstadoProblemaNavigation).WithMany(p => p.ProblemasReportes)
                .HasForeignKey(d => d.IdEstadoProblema)
                .HasConstraintName("FK__Problemas__idEst__7B5B524B");
        });

        modelBuilder.Entity<Proyecto>(entity =>
        {
            entity.HasKey(e => e.IdProyecto).HasName("PK__Proyecto__D0AF4CB469B6CAD5");

            entity.ToTable("Proyecto");

            entity.Property(e => e.IdProyecto).HasColumnName("idProyecto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.FechaContrato).HasColumnType("datetime");
            entity.Property(e => e.FechaEstimadaTermino).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RFCContratista)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("RFCContratista");
        });

        modelBuilder.Entity<RegistroActividade>(entity =>
        {
            entity.HasKey(e => e.IdRegistroActividades).HasName("PK__Registro__66677B9E160FD8FF");

            entity.Property(e => e.IdRegistroActividades).HasColumnName("idRegistroActividades");
            entity.Property(e => e.Campo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DatoAnterior)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DatoNuevo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HoraFecha).HasColumnType("datetime");
            entity.Property(e => e.IdRegistro).HasColumnName("idRegistro");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Tabla)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.RegistroActividades)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__RegistroA__idUsu__6C190EBB");
        });

        modelBuilder.Entity<ReporteDiario>(entity =>
        {
            entity.HasKey(e => e.IdReporte).HasName("PK__Reportes__40D65D3C366148DF");

            entity.Property(e => e.IdReporte).HasColumnName("idReporte");
            entity.Property(e => e.ComentariosResidente)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.IdFrenteObra).HasColumnName("idFrenteObra");
            entity.Property(e => e.IdResidente).HasColumnName("idResidente");
            entity.Property(e => e.IdSupervisor).HasColumnName("idSupervisor");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.ReportesDiarios)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__ReportesD__idEst__72C60C4A");

            entity.HasOne(d => d.IdFrenteObraNavigation).WithMany(p => p.ReportesDiarios)
                .HasForeignKey(d => d.IdFrenteObra)
                .HasConstraintName("FK__ReportesD__idFre__71D1E811");

            entity.HasOne(d => d.IdResidenteNavigation).WithMany(p => p.ReportesDiarioIdResidenteNavigations)
                .HasForeignKey(d => d.IdResidente)
                .HasConstraintName("FK__ReportesD__idRes__73BA3083");

            entity.HasOne(d => d.IdSupervisorNavigation).WithMany(p => p.ReportesDiarioIdSupervisorNavigations)
                .HasForeignKey(d => d.IdSupervisor)
                .HasConstraintName("FK__ReportesD__idSup__70DDC3D8");
        });

        modelBuilder.Entity<TipoConcepto>(entity =>
        {
            entity.HasKey(e => e.IdTipoConcepto).HasName("PK__TipoConc__759B2F4DE5589771");

            entity.ToTable("TipoConcepto");

            entity.Property(e => e.IdTipoConcepto).HasColumnName("idTipoConcepto");
            entity.Property(e => e.TipoConcepto1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TipoConcepto");
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.IdTipoUsuario).HasName("PK__TipoUsua__03006BFF6EA60F5A");

            entity.ToTable("TipoUsuario");

            entity.Property(e => e.IdTipoUsuario).HasColumnName("idTipoUsuario");
            entity.Property(e => e.TipoUsuario1)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("TipoUsuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__645723A6D1059F0D");

            entity.ToTable("Usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.ApellidoM)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ApellidoP)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IdTipoUsuario).HasColumnName("idTipoUsuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Rfc)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("RFC");

            entity.HasOne(d => d.IdTipoUsuarioNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdTipoUsuario)
                .HasConstraintName("FK__Usuario__idTipoU__5EBF139D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
