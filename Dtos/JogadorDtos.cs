using System.ComponentModel.DataAnnotations;

namespace CampeonatoFut7.Dtos;

public class JogadorRequest
{
    [Required(ErrorMessage = "O nome do jogador é obrigatório.")]
    [StringLength(50, ErrorMessage = "O nome do jogador deve ter no máximo 50 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Range(1, 99, ErrorMessage = "O número da camisa deve estar entre 1 e 99.")]
    public int Numero { get; set; }
}

public class JogadorResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Numero { get; set; }
    public int TimeId { get; set; }
}
