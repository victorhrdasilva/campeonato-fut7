namespace CampeonatoFut7.Models;

public class Time
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public List<Jogador> Jogadores { get; set; } = new List<Jogador>();

    public Time(string nome)
    {
        Nome = nome;
    }
}
