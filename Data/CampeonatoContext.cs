using CampeonatoFut7.Models;
using Microsoft.EntityFrameworkCore;

namespace CampeonatoFut7.Data;

public class CampeonatoContext : DbContext
{
    public CampeonatoContext(DbContextOptions<CampeonatoContext> options) : base(options)
    {
    }

    public DbSet<Time> Times => Set<Time>();
    public DbSet<Jogador> Jogadores => Set<Jogador>();
    public DbSet<Partida> Partidas => Set<Partida>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Time>(time =>
        {
            time.Property(t => t.Nome).IsRequired().HasMaxLength(50).UseCollation("NOCASE");
            time.HasIndex(t => t.Nome).IsUnique();
        });

        modelBuilder.Entity<Jogador>(jogador =>
        {
            jogador.Property(j => j.Nome).IsRequired().HasMaxLength(50).UseCollation("NOCASE");

            jogador.HasOne(j => j.Time)
                .WithMany(t => t.Jogadores)
                .HasForeignKey(j => j.TimeId)
                .OnDelete(DeleteBehavior.Cascade);

            jogador.HasIndex(j => new { j.TimeId, j.Nome }).IsUnique();
            jogador.HasIndex(j => new { j.TimeId, j.Numero }).IsUnique();
        });

        modelBuilder.Entity<Partida>(partida =>
        {
            partida.HasOne(p => p.TimeCasa)
                .WithMany()
                .HasForeignKey(p => p.TimeCasaId)
                .OnDelete(DeleteBehavior.Restrict);

            partida.HasOne(p => p.TimeVisitante)
                .WithMany()
                .HasForeignKey(p => p.TimeVisitanteId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
