using System.ComponentModel.DataAnnotations;

namespace CampeonatoFut7.Dtos;

public class TimeRequest
{
    [Required(ErrorMessage = "O nome do time é obrigatório.")]
    [StringLength(50, ErrorMessage = "O nome do time deve ter no máximo 50 caracteres.")]
    public string Nome { get; set; } = string.Empty;
}

public class TimeResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int QuantidadeJogadores { get; set; }
}

public class TimeDetalheResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public List<JogadorResponse> Jogadores { get; set; } = new List<JogadorResponse>();
}
