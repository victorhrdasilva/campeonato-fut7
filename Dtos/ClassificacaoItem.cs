namespace CampeonatoFut7.Dtos;

public class ClassificacaoItem
{
    public int Posicao { get; set; }
    public int TimeId { get; set; }
    public string Time { get; set; } = string.Empty;
    public int Jogos { get; set; }
    public int Vitorias { get; set; }
    public int Empates { get; set; }
    public int Derrotas { get; set; }
    public int GolsPro { get; set; }
    public int GolsContra { get; set; }
    public int SaldoGols => GolsPro - GolsContra;
    public int Pontos { get; set; }
}
