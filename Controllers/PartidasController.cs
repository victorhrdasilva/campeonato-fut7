using CampeonatoFut7.Data;
using CampeonatoFut7.Dtos;
using CampeonatoFut7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampeonatoFut7.Controllers;

[ApiController]
[Route("api/partidas")]
public class PartidasController : ControllerBase
{
    private readonly CampeonatoContext _context;

    public PartidasController(CampeonatoContext context)
    {
        _context = context;
    }

    // UC07 - Listar partidas (mais recentes primeiro)
    [HttpGet]
    public async Task<ActionResult<List<PartidaResponse>>> Listar()
    {
        List<Partida> partidas = await _context.Partidas
            .Include(p => p.TimeCasa)
            .Include(p => p.TimeVisitante)
            .OrderByDescending(p => p.Id)
            .ToListAsync();

        List<PartidaResponse> resposta = new List<PartidaResponse>();

        foreach (Partida partida in partidas)
        {
            resposta.Add(Converter(partida));
        }

        return resposta;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PartidaResponse>> Obter(int id)
    {
        Partida? partida = await _context.Partidas
            .Include(p => p.TimeCasa)
            .Include(p => p.TimeVisitante)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (partida == null)
        {
            return Problem(detail: "Partida não encontrada.", statusCode: 404);
        }

        return Converter(partida);
    }

    // UC06 - Registrar resultado de uma partida
    [HttpPost]
    public async Task<ActionResult<PartidaResponse>> Registrar(PartidaRequest request)
    {
        if (request.TimeCasaId == request.TimeVisitanteId)
        {
            return Problem(detail: "Um time não pode jogar contra ele mesmo.", statusCode: 400);
        }

        Time? casa = await _context.Times.FindAsync(request.TimeCasaId);
        if (casa == null)
        {
            return Problem(detail: "Time da casa não encontrado.", statusCode: 404);
        }

        Time? visitante = await _context.Times.FindAsync(request.TimeVisitanteId);
        if (visitante == null)
        {
            return Problem(detail: "Time visitante não encontrado.", statusCode: 404);
        }

        int jogadoresCasa = await _context.Jogadores.CountAsync(j => j.TimeId == casa.Id);
        if (jogadoresCasa < RegrasCampeonato.MinJogadoresParaPartida)
        {
            return Problem(
                detail: "O time " + casa.Nome + " precisa de pelo menos "
                    + RegrasCampeonato.MinJogadoresParaPartida
                    + " jogadores para disputar uma partida (tem " + jogadoresCasa + ").",
                statusCode: 409);
        }

        int jogadoresVisitante = await _context.Jogadores.CountAsync(j => j.TimeId == visitante.Id);
        if (jogadoresVisitante < RegrasCampeonato.MinJogadoresParaPartida)
        {
            return Problem(
                detail: "O time " + visitante.Nome + " precisa de pelo menos "
                    + RegrasCampeonato.MinJogadoresParaPartida
                    + " jogadores para disputar uma partida (tem " + jogadoresVisitante + ").",
                statusCode: 409);
        }

        Partida partida = new Partida(
            request.TimeCasaId,
            request.TimeVisitanteId,
            request.GolsCasa,
            request.GolsVisitante);

        _context.Partidas.Add(partida);
        await _context.SaveChangesAsync();

        partida.TimeCasa = casa;
        partida.TimeVisitante = visitante;

        return CreatedAtAction(nameof(Obter), new { id = partida.Id }, Converter(partida));
    }

    // UC12 - Anular partida lançada por engano (a classificação é recalculada)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Anular(int id)
    {
        Partida? partida = await _context.Partidas.FindAsync(id);
        if (partida == null)
        {
            return Problem(detail: "Partida não encontrada.", statusCode: 404);
        }

        _context.Partidas.Remove(partida);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static PartidaResponse Converter(Partida partida)
    {
        string casa = partida.TimeCasa?.Nome ?? string.Empty;
        string visitante = partida.TimeVisitante?.Nome ?? string.Empty;

        string resultado;
        if (partida.GolsCasa > partida.GolsVisitante)
        {
            resultado = "Vitória do " + casa;
        }
        else if (partida.GolsCasa < partida.GolsVisitante)
        {
            resultado = "Vitória do " + visitante;
        }
        else
        {
            resultado = "Empate";
        }

        return new PartidaResponse
        {
            Id = partida.Id,
            DataHora = partida.DataHora,
            TimeCasaId = partida.TimeCasaId,
            TimeCasa = casa,
            TimeVisitanteId = partida.TimeVisitanteId,
            TimeVisitante = visitante,
            GolsCasa = partida.GolsCasa,
            GolsVisitante = partida.GolsVisitante,
            Resultado = resultado
        };
    }
}
