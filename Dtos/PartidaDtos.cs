using System.ComponentModel.DataAnnotations;

namespace CampeonatoFut7.Dtos;

public class PartidaRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Informe o id do time da casa.")]
    public int TimeCasaId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Informe o id do time visitante.")]
    public int TimeVisitanteId { get; set; }

    [Range(0, 99, ErrorMessage = "Os gols do time da casa devem estar entre 0 e 99.")]
    public int GolsCasa { get; set; }

    [Range(0, 99, ErrorMessage = "Os gols do time visitante devem estar entre 0 e 99.")]
    public int GolsVisitante { get; set; }
}

public class PartidaResponse
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public int TimeCasaId { get; set; }
    public string TimeCasa { get; set; } = string.Empty;
    public int TimeVisitanteId { get; set; }
    public string TimeVisitante { get; set; } = string.Empty;
    public int GolsCasa { get; set; }
    public int GolsVisitante { get; set; }
    public string Resultado { get; set; } = string.Empty;
}
