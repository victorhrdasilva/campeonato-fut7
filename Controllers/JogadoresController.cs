using CampeonatoFut7.Data;
using CampeonatoFut7.Dtos;
using CampeonatoFut7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampeonatoFut7.Controllers;

[ApiController]
[Route("api")]
public class JogadoresController : ControllerBase
{
    private readonly CampeonatoContext _context;

    public JogadoresController(CampeonatoContext context)
    {
        _context = context;
    }

    // UC05 - Listar jogadores de um time
    [HttpGet("times/{timeId:int}/jogadores")]
    public async Task<ActionResult<List<JogadorResponse>>> Listar(int timeId)
    {
        bool timeExiste = await _context.Times.AnyAsync(t => t.Id == timeId);
        if (!timeExiste)
        {
            return Problem(detail: "Time não encontrado.", statusCode: 404);
        }

        List<JogadorResponse> jogadores = await _context.Jogadores
            .Where(j => j.TimeId == timeId)
            .OrderBy(j => j.Numero)
            .Select(j => new JogadorResponse
            {
                Id = j.Id,
                Nome = j.Nome,
                Numero = j.Numero,
                TimeId = j.TimeId
            })
            .ToListAsync();

        return jogadores;
    }

    // UC04 - Cadastrar jogador em um time
    [HttpPost("times/{timeId:int}/jogadores")]
    public async Task<ActionResult<JogadorResponse>> Cadastrar(int timeId, JogadorRequest request)
    {
        bool timeExiste = await _context.Times.AnyAsync(t => t.Id == timeId);
        if (!timeExiste)
        {
            return Problem(detail: "Time não encontrado.", statusCode: 404);
        }

        string nome = request.Nome.Trim();

        List<Jogador> elenco = await _context.Jogadores
            .Where(j => j.TimeId == timeId)
            .ToListAsync();

        if (elenco.Count >= RegrasCampeonato.MaxJogadoresPorTime)
        {
            return Problem(
                detail: "O time já atingiu o limite de "
                    + RegrasCampeonato.MaxJogadoresPorTime + " jogadores.",
                statusCode: 409);
        }

        foreach (Jogador existente in elenco)
        {
            if (string.Equals(existente.Nome, nome, StringComparison.OrdinalIgnoreCase))
            {
                return Problem(detail: "Esse jogador já está cadastrado.", statusCode: 409);
            }

            if (existente.Numero == request.Numero)
            {
                return Problem(detail: "Já existe um jogador com esse número.", statusCode: 409);
            }
        }

        Jogador jogador = new Jogador(nome, request.Numero);
        jogador.TimeId = timeId;

        _context.Jogadores.Add(jogador);
        await _context.SaveChangesAsync();

        JogadorResponse resposta = new JogadorResponse
        {
            Id = jogador.Id,
            Nome = jogador.Nome,
            Numero = jogador.Numero,
            TimeId = jogador.TimeId
        };

        return CreatedAtAction(nameof(Listar), new { timeId = timeId }, resposta);
    }

    // UC11 - Remover jogador
    [HttpDelete("jogadores/{id:int}")]
    public async Task<IActionResult> Remover(int id)
    {
        Jogador? jogador = await _context.Jogadores.FindAsync(id);
        if (jogador == null)
        {
            return Problem(detail: "Jogador não encontrado.", statusCode: 404);
        }

        _context.Jogadores.Remove(jogador);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
