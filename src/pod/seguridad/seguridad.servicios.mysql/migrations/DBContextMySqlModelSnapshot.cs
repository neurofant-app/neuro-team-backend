﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using seguridad.servicios.mysql;

#nullable disable

namespace seguridad.servicios.mysql.migrations
{
    [DbContext(typeof(DBContextMySql))]
    partial class DBContextMySqlModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("seguridad.modelo.Aplicacion", b =>
                {
                    b.Property<Guid>("ApplicacionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Descripcion")
                        .HasColumnType("longtext");

                    b.Property<string>("Nombre")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("ApplicacionId");

                    b.ToTable("seguridad$aplicacion", (string)null);
                });

            modelBuilder.Entity("seguridad.modelo.GrupoUsuarios", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AplicacionApplicacionId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ApplicacionId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Descripcion")
                        .HasColumnType("longtext");

                    b.Property<string>("DominioId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("UsuarioId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AplicacionApplicacionId");

                    b.ToTable("seguridad$grupousuarios", (string)null);
                });

            modelBuilder.Entity("seguridad.modelo.Modulo", b =>
                {
                    b.Property<string>("ModuloId")
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("ApplicacionId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Descripcion")
                        .HasColumnType("longtext");

                    b.Property<string>("Nombre")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.HasKey("ModuloId");

                    b.HasIndex("ApplicacionId");

                    b.ToTable("seguridad$modulo", (string)null);
                });

            modelBuilder.Entity("seguridad.modelo.Permiso", b =>
                {
                    b.Property<string>("PermisoId")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Ambito")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .HasColumnType("longtext");

                    b.Property<string>("ModuloId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Nombre")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("RolId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("PermisoId");

                    b.HasIndex("ModuloId");

                    b.HasIndex("RolId");

                    b.ToTable("seguridad$permiso", (string)null);
                });

            modelBuilder.Entity("seguridad.modelo.Rol", b =>
                {
                    b.Property<string>("RolId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Descripcion")
                        .HasColumnType("longtext");

                    b.Property<string>("InstanciaAplicacionId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ModuloId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Nombre")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Permisos")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("Personalizado")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("RolId");

                    b.HasIndex("InstanciaAplicacionId");

                    b.HasIndex("ModuloId");

                    b.ToTable("seguridad$rol", (string)null);
                });

            modelBuilder.Entity("seguridad.modelo.UsuarioGrupo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("GrupoId")
                        .HasColumnType("char(36)");

                    b.Property<string>("UsuarioId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("GrupoId");

                    b.ToTable("seguridad$usuariogrupo", (string)null);
                });

            modelBuilder.Entity("seguridad.modelo.instancias.InstanciaAplicacion", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("ApplicacionId")
                        .HasColumnType("char(36)");

                    b.Property<string>("DominioId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicacionId")
                        .IsUnique();

                    b.ToTable("seguridad$instanciaaplicacion", (string)null);
                });

            modelBuilder.Entity("seguridad.modelo.relaciones.PermisoGrupo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("GrupoId")
                        .HasColumnType("char(36)");

                    b.Property<string>("PermisoId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("GrupoId");

                    b.HasIndex("PermisoId");

                    b.ToTable("seguridad$permisoGrupo", (string)null);
                });

            modelBuilder.Entity("seguridad.modelo.relaciones.PermisoUsuario", b =>
                {
                    b.Property<string>("UsuarioId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PermisoId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("UsuarioId", "PermisoId");

                    b.HasIndex("Id");

                    b.HasIndex("PermisoId");

                    b.ToTable("seguridad$permisoUsuario", (string)null);
                });

            modelBuilder.Entity("seguridad.modelo.relaciones.RolGrupo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("GrupoId")
                        .HasColumnType("char(36)");

                    b.Property<string>("RolId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("GrupoId");

                    b.HasIndex("RolId");

                    b.ToTable("seguridad$rolgrupo", (string)null);
                });

            modelBuilder.Entity("seguridad.modelo.relaciones.RolUsuario", b =>
                {
                    b.Property<string>("UsuarioId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RolId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("UsuarioId", "RolId");

                    b.HasIndex("Id");

                    b.HasIndex("RolId");

                    b.ToTable("seguridad$rolUsuario", (string)null);
                });

            modelBuilder.Entity("seguridad.modelo.GrupoUsuarios", b =>
                {
                    b.HasOne("seguridad.modelo.Aplicacion", "Aplicacion")
                        .WithMany()
                        .HasForeignKey("AplicacionApplicacionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aplicacion");
                });

            modelBuilder.Entity("seguridad.modelo.Modulo", b =>
                {
                    b.HasOne("seguridad.modelo.Aplicacion", "Aplicacion")
                        .WithMany("Modulos")
                        .HasForeignKey("ApplicacionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aplicacion");
                });

            modelBuilder.Entity("seguridad.modelo.Permiso", b =>
                {
                    b.HasOne("seguridad.modelo.Modulo", "Modulo")
                        .WithMany("Permisos")
                        .HasForeignKey("ModuloId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("seguridad.modelo.Rol", "Rol")
                        .WithMany("RolPermisos")
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Modulo");

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("seguridad.modelo.Rol", b =>
                {
                    b.HasOne("seguridad.modelo.instancias.InstanciaAplicacion", "InstanciaAplicacion")
                        .WithMany("RolesPersonalizados")
                        .HasForeignKey("InstanciaAplicacionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("seguridad.modelo.Modulo", "Modulo")
                        .WithMany("RolesPredefinidos")
                        .HasForeignKey("ModuloId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InstanciaAplicacion");

                    b.Navigation("Modulo");
                });

            modelBuilder.Entity("seguridad.modelo.UsuarioGrupo", b =>
                {
                    b.HasOne("seguridad.modelo.GrupoUsuarios", "GrupoUsuarios")
                        .WithMany("UsuariosId")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GrupoUsuarios");
                });

            modelBuilder.Entity("seguridad.modelo.instancias.InstanciaAplicacion", b =>
                {
                    b.HasOne("seguridad.modelo.Aplicacion", "Aplicacion")
                        .WithOne("InstanciaAplicacion")
                        .HasForeignKey("seguridad.modelo.instancias.InstanciaAplicacion", "ApplicacionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aplicacion");
                });

            modelBuilder.Entity("seguridad.modelo.relaciones.PermisoGrupo", b =>
                {
                    b.HasOne("seguridad.modelo.GrupoUsuarios", "Grupo")
                        .WithMany("PermisoGrupo")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("seguridad.modelo.instancias.InstanciaAplicacion", "InstanciaAplicacion")
                        .WithMany("PermisoGrupo")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("seguridad.modelo.Permiso", "Permiso")
                        .WithMany("PermisoGrupo")
                        .HasForeignKey("PermisoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grupo");

                    b.Navigation("InstanciaAplicacion");

                    b.Navigation("Permiso");
                });

            modelBuilder.Entity("seguridad.modelo.relaciones.PermisoUsuario", b =>
                {
                    b.HasOne("seguridad.modelo.instancias.InstanciaAplicacion", "InstanciaAplicacion")
                        .WithMany("PermisoUsuarios")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("seguridad.modelo.Permiso", "Permiso")
                        .WithMany("PermisoUsuario")
                        .HasForeignKey("PermisoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InstanciaAplicacion");

                    b.Navigation("Permiso");
                });

            modelBuilder.Entity("seguridad.modelo.relaciones.RolGrupo", b =>
                {
                    b.HasOne("seguridad.modelo.GrupoUsuarios", "Grupo")
                        .WithMany("RolGrupo")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("seguridad.modelo.instancias.InstanciaAplicacion", "InstanciaAplicacion")
                        .WithMany("RolGrupo")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("seguridad.modelo.Rol", "Rol")
                        .WithMany("RolGrupo")
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grupo");

                    b.Navigation("InstanciaAplicacion");

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("seguridad.modelo.relaciones.RolUsuario", b =>
                {
                    b.HasOne("seguridad.modelo.instancias.InstanciaAplicacion", "InstanciaAplicacion")
                        .WithMany("RolUsuarios")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("seguridad.modelo.Rol", "Rol")
                        .WithMany("RolUsuario")
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InstanciaAplicacion");

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("seguridad.modelo.Aplicacion", b =>
                {
                    b.Navigation("InstanciaAplicacion")
                        .IsRequired();

                    b.Navigation("Modulos");
                });

            modelBuilder.Entity("seguridad.modelo.GrupoUsuarios", b =>
                {
                    b.Navigation("PermisoGrupo");

                    b.Navigation("RolGrupo");

                    b.Navigation("UsuariosId");
                });

            modelBuilder.Entity("seguridad.modelo.Modulo", b =>
                {
                    b.Navigation("Permisos");

                    b.Navigation("RolesPredefinidos");
                });

            modelBuilder.Entity("seguridad.modelo.Permiso", b =>
                {
                    b.Navigation("PermisoGrupo");

                    b.Navigation("PermisoUsuario");
                });

            modelBuilder.Entity("seguridad.modelo.Rol", b =>
                {
                    b.Navigation("RolGrupo");

                    b.Navigation("RolPermisos");

                    b.Navigation("RolUsuario");
                });

            modelBuilder.Entity("seguridad.modelo.instancias.InstanciaAplicacion", b =>
                {
                    b.Navigation("PermisoGrupo");

                    b.Navigation("PermisoUsuarios");

                    b.Navigation("RolGrupo");

                    b.Navigation("RolUsuarios");

                    b.Navigation("RolesPersonalizados");
                });
#pragma warning restore 612, 618
        }
    }
}
