using CampeonatoFut7.Data;
using CampeonatoFut7.Dtos;
using CampeonatoFut7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampeonatoFut7.Controllers;

[ApiController]
[Route("api/times")]
public class TimesController : ControllerBase
{
    private readonly CampeonatoContext _context;

    public TimesController(CampeonatoContext context)
    {
        _context = context;
    }

    // UC02 - Listar times
    [HttpGet]
    public async Task<ActionResult<List<TimeResponse>>> Listar()
    {
        List<TimeResponse> times = await _context.Times
            .OrderBy(t => t.Nome)
            .Select(t => new TimeResponse
            {
                Id = t.Id,
                Nome = t.Nome,
                QuantidadeJogadores = t.Jogadores.Count
            })
            .ToListAsync();

        return times;
    }

    // UC03 - Buscar time pelo nome (busca parcial, sem diferenciar maiúsculas)
    [HttpGet("buscar")]
    public async Task<ActionResult<List<TimeResponse>>> Buscar([FromQuery] string? nome = null)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            return Problem(detail: "Informe o nome do time para a busca.", statusCode: 400);
        }

        string padrao = "%" + nome.Trim() + "%";

        List<TimeResponse> times = await _context.Times
            .Where(t => EF.Functions.Like(t.Nome, padrao))
            .OrderBy(t => t.Nome)
            .Select(t => new TimeResponse
            {
                Id = t.Id,
                Nome = t.Nome,
                QuantidadeJogadores = t.Jogadores.Count
            })
            .ToListAsync();

        return times;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TimeDetalheResponse>> Obter(int id)
    {
        Time? time = await _context.Times
            .Include(t => t.Jogadores)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (time == null)
        {
            return Problem(detail: "Time não encontrado.", statusCode: 404);
        }

        TimeDetalheResponse resposta = new TimeDetalheResponse
        {
            Id = time.Id,
            Nome = time.Nome
        };

        foreach (Jogador jogador in time.Jogadores.OrderBy(j => j.Numero))
        {
            resposta.Jogadores.Add(new JogadorResponse
            {
                Id = jogador.Id,
                Nome = jogador.Nome,
                Numero = jogador.Numero,
                TimeId = jogador.TimeId
            });
        }

        return resposta;
    }

    // UC01 - Cadastrar time
    [HttpPost]
    public async Task<ActionResult<TimeResponse>> Cadastrar(TimeRequest request)
    {
        string nome = request.Nome.Trim();

        bool existe = await _context.Times.AnyAsync(t => t.Nome == nome);
        if (existe)
        {
            return Problem(detail: "Esse time já está cadastrado.", statusCode: 409);
        }

        Time time = new Time(nome);
        _context.Times.Add(time);
        await _context.SaveChangesAsync();

        TimeResponse resposta = new TimeResponse
        {
            Id = time.Id,
            Nome = time.Nome,
            QuantidadeJogadores = 0
        };

        return CreatedAtAction(nameof(Obter), new { id = time.Id }, resposta);
    }

    // UC09 - Renomear time
    [HttpPut("{id:int}")]
    public async Task<ActionResult<TimeResponse>> Renomear(int id, TimeRequest request)
    {
        Time? time = await _context.Times.FindAsync(id);
        if (time == null)
        {
            return Problem(detail: "Time não encontrado.", statusCode: 404);
        }

        string nome = request.Nome.Trim();

        bool existe = await _context.Times.AnyAsync(t => t.Nome == nome && t.Id != id);
        if (existe)
        {
            return Problem(detail: "Já existe outro time com esse nome.", statusCode: 409);
        }

        time.Nome = nome;
        await _context.SaveChangesAsync();

        int quantidade = await _context.Jogadores.CountAsync(j => j.TimeId == id);

        return new TimeResponse
        {
            Id = time.Id,
            Nome = time.Nome,
            QuantidadeJogadores = quantidade
        };
    }

    // UC10 - Remover time (somente se ainda não disputou partidas)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int id)
    {
        Time? time = await _context.Times.FindAsync(id);
        if (time == null)
        {
            return Problem(detail: "Time não encontrado.", statusCode: 404);
        }

        bool temPartidas = await _context.Partidas
            .AnyAsync(p => p.TimeCasaId == id || p.TimeVisitanteId == id);

        if (temPartidas)
        {
            return Problem(
                detail: "Não é possível remover um time que já disputou partidas.",
                statusCode: 409);
        }

        _context.Times.Remove(time);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
