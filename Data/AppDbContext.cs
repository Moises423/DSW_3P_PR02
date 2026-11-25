using DSW_3P_PR02.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR02.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Personaje> Personajes => Set<Personaje>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var personaje = modelBuilder.Entity<Personaje>();

        personaje.ToTable("Personajes");
        personaje.HasKey(p => p.Id);
        personaje.Property(p => p.Id).ValueGeneratedOnAdd();

        personaje.Property(p => p.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        personaje.Property(p => p.Videojuego)
            .IsRequired()
            .HasMaxLength(150)
            .UseCollation("Latin1_General_100_CI_AS");

        personaje.Property(p => p.Clase)
            .IsRequired()
            .HasMaxLength(100);

        personaje.Property(p => p.Nivel).IsRequired();
        personaje.Property(p => p.Poder).IsRequired();
        personaje.Property(p => p.Disponible).HasDefaultValue(true);
        personaje.Property(p => p.ImagenUrl).HasMaxLength(2048);

        personaje.HasData(
            new Personaje
            {
                Id = 1,
                Nombre = "Lara Croft",
                Videojuego = "Tomb Raider",
                Clase = "Exploradora",
                Nivel = 95,
                Poder = 90,
                Disponible = true
            },
            new Personaje
            {
                Id = 2,
                Nombre = "Link",
                Videojuego = "The Legend of Zelda",
                Clase = "Espadachín",
                Nivel = 85,
                Poder = 88,
                Disponible = true
            },
            new Personaje
            {
                Id = 3,
                Nombre = "Samus Aran",
                Videojuego = "Metroid",
                Clase = "Cazarrecompensas",
                Nivel = 92,
                Poder = 96,
                Disponible = true
            },
            new Personaje
            {
                Id = 4,
                Nombre = "Geralt de Rivia",
                Videojuego = "The Witcher 3",
                Clase = "Brujo",
                Nivel = 90,
                Poder = 93,
                Disponible = true
            });
    }
}
