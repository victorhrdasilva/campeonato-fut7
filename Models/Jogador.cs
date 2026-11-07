namespace CampeonatoFut7.Models;

public class Jogador
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Numero { get; set; }

    public int TimeId { get; set; }
    public Time? Time { get; set; }

    public Jogador(string nome, int numero)
    {
        Nome = nome;
        Numero = numero;
    }
}
