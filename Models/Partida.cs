namespace CampeonatoFut7.Models;

public class Partida
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; } = DateTime.UtcNow;

    public int TimeCasaId { get; set; }
    public Time? TimeCasa { get; set; }

    public int TimeVisitanteId { get; set; }
    public Time? TimeVisitante { get; set; }

    public int GolsCasa { get; set; }
    public int GolsVisitante { get; set; }

    public Partida(int timeCasaId, int timeVisitanteId, int golsCasa, int golsVisitante)
    {
        TimeCasaId = timeCasaId;
        TimeVisitanteId = timeVisitanteId;
        GolsCasa = golsCasa;
        GolsVisitante = golsVisitante;
    }
}
