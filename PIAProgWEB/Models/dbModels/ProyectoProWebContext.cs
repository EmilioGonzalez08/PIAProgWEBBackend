using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PIAProgWEB.Models.dbModels
{
    public partial class ProyectoProWebContext : IdentityDbContext <ApplicationUser, IdentityRole<int>, int>
    {
        public ProyectoProWebContext()
        {
        }

        public ProyectoProWebContext(DbContextOptions<ProyectoProWebContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Carrito> Carritos { get; set; } = null!;
        public virtual DbSet<Categorium> Categoria { get; set; } = null!;
        public virtual DbSet<DetalleOrden> DetalleOrdens { get; set; } = null!;
        public virtual DbSet<DirecciónEnvio> DirecciónEnvios { get; set; } = null!;
        public virtual DbSet<Estacione> Estaciones { get; set; } = null!;
        public virtual DbSet<EstadoOrden> EstadoOrdens { get; set; } = null!;
        public virtual DbSet<ImagenNovedad> ImagenNovedads { get; set; } = null!;
        public virtual DbSet<Novedad> Novedads { get; set; } = null!;
        public virtual DbSet<Orden> Ordens { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;
        public virtual DbSet<ProductoTalla> ProductoTallas { get; set; } = null!;
        public virtual DbSet<Subcategorium> Subcategoria { get; set; } = null!;
        public virtual DbSet<Talla> Tallas { get; set; } = null!;

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Carrito>(entity =>
            {
                entity.HasKey(e => new { e.UsuarioId, e.ProductioId });

                entity.HasOne(d => d.Productio)
                    .WithMany(p => p.Carritos)
                    .HasForeignKey(d => d.ProductioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Carrito_Producto");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Carritos)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Carrito_Usuario");
            });

            modelBuilder.Entity<DetalleOrden>(entity =>
            {
                entity.HasKey(e => new { e.OrdenId, e.ProductoId });

                entity.HasOne(d => d.Orden)
                    .WithMany(p => p.DetalleOrdens)
                    .HasForeignKey(d => d.OrdenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Detalle_orden_Orden");

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.DetalleOrdens)
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Detalle_orden_Producto");
            });

            modelBuilder.Entity<DirecciónEnvio>(entity =>
            {
                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.DirecciónEnvios)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dirección_envio_Usuario");
            });

            modelBuilder.Entity<ImagenNovedad>(entity =>
            {
                entity.HasOne(d => d.IdNovedadNavigation)
                    .WithMany(p => p.ImagenNovedads)
                    .HasForeignKey(d => d.IdNovedad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Imagen_novedad_Novedad");
            });

            modelBuilder.Entity<Novedad>(entity =>
            {
                entity.HasOne(d => d.IdEstacionNavigation)
                    .WithMany(p => p.Novedads)
                    .HasForeignKey(d => d.IdEstacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Novedad_Estaciones");
            });

            modelBuilder.Entity<Orden>(entity =>
            {
                entity.HasOne(d => d.EstadoOrdenNavigation)
                    .WithMany(p => p.Ordens)
                    .HasForeignKey(d => d.EstadoOrden)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orden_Estado_orden");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Ordens)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orden_Usuario");
            });

            modelBuilder.Entity<ProductoTalla>(entity =>
            {
                entity.HasKey(e => new { e.TallaId, e.ProductoId });

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.ProductoTallas)
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductoTalla_Producto");

                entity.HasOne(d => d.Talla)
                    .WithMany(p => p.ProductoTallas)
                    .HasForeignKey(d => d.TallaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductoTalla_Talla");
            });

            modelBuilder.Entity<Subcategorium>(entity =>
            {
                entity.HasKey(e => e.IdSubcategoria)
                    .HasName("PK_Genero");

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.Subcategoria)
                    .HasForeignKey(d => d.CategoriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Subcategoria_Categoria");
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
