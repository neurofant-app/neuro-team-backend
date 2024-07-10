using Microsoft.EntityFrameworkCore;
using seguridad.modelo;
using seguridad.modelo.instancias;


namespace seguridad.servicios.mysql;


public class DBContextMySql : DbContext
{
    public const string TablaAplicacion = "seguridad$aplicacion";
    public const string TablaInstanciaAplicaion = "seguridad$instanciaaplicacion";
    public const string TablaGrupoUsuarios = "seguridad$grupousuarios";
    public const string TablaUsuarioGrupo = "seguridad$usuariogrupo";
    public const string TablaRol = "seguridad$rol";


    public DbSet<Aplicacion> Aplicacion { get; set; }
    public DbSet<InstanciaAplicacion> InstanciaAplicacion { get; set; }
    public DbSet<GrupoUsuarios> GrupoUsuarios { get; set; }
    public DbSet<UsuarioGrupo> UsuarioGrupo { get; set; }
    public DbSet<Rol> Rol { get; set; }
    public DbSet<Permiso> Permiso { get; set; }
    public DbSet<Modulo> Modulo { get; set; }

    public DBContextMySql(DbContextOptions<DBContextMySql> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ConfiguracionAplicacion());
        modelBuilder.ApplyConfiguration(new ConfiguracionInstanciaAplicacion());
        modelBuilder.ApplyConfiguration(new ConfiguracionGrupoUsuarios());
        modelBuilder.ApplyConfiguration(new ConfiguracionUsuarioGrupo());
        modelBuilder.ApplyConfiguration(new ConfiguracionRol());
        modelBuilder.ApplyConfiguration(new ConfiguracionModulo());
        modelBuilder.ApplyConfiguration(new ConfiguracionPermiso());
        modelBuilder.ApplyConfiguration(new ConfiguracionRolUsuario());
        modelBuilder.ApplyConfiguration(new ConfiguracionRolGrupo());
        modelBuilder.ApplyConfiguration(new ConfiguracionPermisoUsuario());
        modelBuilder.ApplyConfiguration(new ConfiguracionPermisoGrupo());
        base.OnModelCreating(modelBuilder);
    }
}